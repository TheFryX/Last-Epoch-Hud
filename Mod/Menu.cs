using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Mod.Cheats;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static UnityEngine.GUI;
using Newtonsoft.Json;
using System;

namespace Mod
{
    internal class Menu
    {
        static Menu()
        {
            LoadSettings();
            Debug.Log("[NPC Mod] Menu initialized");
        }

        private static bool guiVisible = false;
        private const float resizeGripSize = 20f;
        private static bool isResizing = false;
        private static Vector2 resizeStartMouse;
        private static Vector2 resizeStartSize;
        private static Vector2 scrollPosition;

        // Skill inputs
        private static string inputQ = Settings.skillQ.ToString();
        private static string inputW = Settings.skillW.ToString();
        private static string inputE = Settings.skillE.ToString();
        private static string inputR = Settings.skillR.ToString();

        // Auto clicker timers
        private static Dictionary<KeyCode, float> nextAutoClickTime = new Dictionary<KeyCode, float>
        {
            { KeyCode.Q, 0f }, { KeyCode.W, 0f }, { KeyCode.E, 0f }, { KeyCode.R, 0f }
        };

        // Window settings
        public static Rect windowRect = new Rect(
            Settings.windowX,
            Settings.windowY,
            Mathf.Clamp(Settings.windowWidth, 300, Screen.width),
            Mathf.Clamp(Settings.windowHeight, 400, Screen.height)
        );

        public static void SaveSettings()
        {
            try
            {
                if (float.TryParse(inputQ, out float q)) Settings.skillQ = q;
                if (float.TryParse(inputW, out float w)) Settings.skillW = w;
                if (float.TryParse(inputE, out float e)) Settings.skillE = e;
                if (float.TryParse(inputR, out float r)) Settings.skillR = r;

                Settings.windowX = windowRect.x;
                Settings.windowY = windowRect.y;
                Settings.windowWidth = windowRect.width;
                Settings.windowHeight = windowRect.height;

                Settings.SaveSettings();
            }
            catch (Exception ex)
            {
                Debug.LogError("[NPC Mod] Save error: " + ex.Message);
            }
        }

        public static void LoadSettings()
        {
            Settings.LoadSettings();

            inputQ = Settings.skillQ.ToString();
            inputW = Settings.skillW.ToString();
            inputE = Settings.skillE.ToString();
            inputR = Settings.skillR.ToString();

            windowRect = new Rect(
                Settings.windowX,
                Settings.windowY,
                Mathf.Clamp(Settings.windowWidth, 300, Screen.width),
                Mathf.Clamp(Settings.windowHeight, 400, Screen.height)
            );
        }

        public static void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                guiVisible = !guiVisible;
            }

            foreach (KeyCode key in new[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R })
            {
                if (!IsAutoEnabled(key)) continue;

                if (Time.time >= nextAutoClickTime[key])
                {
                    SimulateKeyPress(key);
                    nextAutoClickTime[key] = Time.time + GetSkillCooldown(key);
                }
            }
        }

        public static void DrawModWindow(int windowID)
        {
            // Zapewnij minimalny rozmiar okna
            windowRect.width = Mathf.Clamp(windowRect.width, 300, Screen.width);
            windowRect.height = Mathf.Clamp(windowRect.height, 400, Screen.height);

            GUILayout.BeginVertical(GUILayout.Height(windowRect.height));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            // Risky Options Section
            Settings.riskyOptionsDropdown = GUILayout.Toggle(Settings.riskyOptionsDropdown, "⚠️ Risky Options ⚠️", "button");
            if (Settings.riskyOptionsDropdown)
            {
                GUILayout.Label("Time Scale: " + Settings.timeScale.ToString("F1"));
                Settings.timeScale = GUILayout.HorizontalSlider(Settings.timeScale, 0.1f, 3.0f);

                Settings.useAnyWaypoint = GUILayout.Toggle(Settings.useAnyWaypoint, "Use Any Waypoint");
                //Settings.pickupCrafting = GUILayout.Toggle(Settings.pickupCrafting, "Pickup Crafting Items");
            }

            // Skills Section
            Settings.skillsDropdown = GUILayout.Toggle(Settings.skillsDropdown, "Auto Caster", "button");
            if (Settings.skillsDropdown)
            {
                GUILayout.Label("Cooldown (seconds):");
                DrawSkillInput("Q", KeyCode.Q, ref inputQ, Settings.autoQ);
                DrawSkillInput("W", KeyCode.W, ref inputW, Settings.autoW);
                DrawSkillInput("E", KeyCode.E, ref inputE, Settings.autoE);
                DrawSkillInput("R", KeyCode.R, ref inputR, Settings.autoR);
            }

            // Auto Pot Section
            Settings.autoPotDistanceDropdown = GUILayout.Toggle(Settings.autoPotDistanceDropdown, "Auto Potion", "button");
            if (Settings.autoPotDistanceDropdown)
            {
                Settings.useAutoPot = GUILayout.Toggle(Settings.useAutoPot, "Enable Auto Potion");
                if (Settings.useAutoPot)
                {
                    GUILayout.Label("HP Threshold: " + Settings.autoHealthPotion.ToString("F0") + "%");
                    Settings.autoHealthPotion = GUILayout.HorizontalSlider(Settings.autoHealthPotion, 0f, 100f);
                }

                GUILayout.Label("Draw Distance: " + Settings.drawDistance.ToString("F0"));
                Settings.drawDistance = GUILayout.HorizontalSlider(Settings.drawDistance, 10f, 300f);
            }

            // NPC Drawings Section
            Settings.npcDrawingsDropdown = GUILayout.Toggle(Settings.npcDrawingsDropdown, "NPC Drawings", "button");
            if (Settings.npcDrawingsDropdown)
            {
                foreach (var entry in Settings.npcDrawings)
                {
                    Settings.npcDrawings[entry.Key] = GUILayout.Toggle(entry.Value, entry.Key);
                }
            }

            // NPC Classifications Section
            Settings.npcClassificationsDropdown = GUILayout.Toggle(Settings.npcClassificationsDropdown, "NPC Classifications", "button");
            if (Settings.npcClassificationsDropdown)
            {
                foreach (var entry in Settings.npcClassifications)
                {
                    Settings.npcClassifications[entry.Key] = GUILayout.Toggle(entry.Value, entry.Key);
                }
            }

            // Item Filter Section
            Settings.itemDrawingsDropdown = GUILayout.Toggle(Settings.itemDrawingsDropdown, "Loot Filter", "button");
            if (Settings.itemDrawingsDropdown)
            {
                Settings.useLootFilter = GUILayout.Toggle(Settings.useLootFilter, "Enable Loot Filter");
                GUI.enabled = !Settings.useLootFilter;

                foreach (var entry in Settings.itemDrawings)
                {
                    Settings.itemDrawings[entry.Key] = GUILayout.Toggle(entry.Value, entry.Key);
                }
                GUI.enabled = true;
            }

            // Game Modifications Section
            Settings.gamePatchesDropdown = GUILayout.Toggle(Settings.gamePatchesDropdown, "Game Modifications", "button");
            if (Settings.gamePatchesDropdown)
            {
                Settings.removeFog = GUILayout.Toggle(Settings.removeFog, "Remove Fog");
                Settings.cameraZoomUnlock = GUILayout.Toggle(Settings.cameraZoomUnlock, "Unlock Camera Zoom");
                Settings.minimapZoomUnlock = GUILayout.Toggle(Settings.minimapZoomUnlock, "Unlock Minimap Zoom");
                Settings.mapHack = GUILayout.Toggle(Settings.mapHack, "Map Hack");
                Settings.playerLantern = GUILayout.Toggle(Settings.playerLantern, "Player Lantern");
            }

            // Save Button
            if (GUILayout.Button("💾 Save Settings"))
            {
                SaveSettings();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            HandleWindowResizing();
            GUI.DragWindow(new Rect(0, 0, windowRect.width - resizeGripSize, 25));
        }

        private static void HandleWindowResizing()
        {
            Rect resizeArea = new Rect(
                windowRect.width - resizeGripSize,
                windowRect.height - resizeGripSize,
                resizeGripSize,
                resizeGripSize
            );

            // Narysuj uchwyt zmiany rozmiaru
            GUI.Box(resizeArea, "", GUI.skin.button);

            Event current = Event.current;

            if (current.type == EventType.MouseDown && resizeArea.Contains(current.mousePosition))
            {
                isResizing = true;
                resizeStartMouse = current.mousePosition;
                resizeStartSize = new Vector2(windowRect.width, windowRect.height);
                current.Use();
            }

            if (isResizing && current.type == EventType.MouseDrag)
            {
                Vector2 mouseDelta = current.mousePosition - resizeStartMouse;
                windowRect.width = Mathf.Clamp(resizeStartSize.x + mouseDelta.x, 300, Screen.width);
                windowRect.height = Mathf.Clamp(resizeStartSize.y + mouseDelta.y, 400, Screen.height);
                current.Use();
            }

            if (current.type == EventType.MouseUp)
            {
                isResizing = false;
            }
        }

        private static void DrawSkillInput(string label, KeyCode key, ref string input, bool autoEnabled)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label + ":", GUILayout.Width(30));
            input = GUILayout.TextField(input, GUILayout.Width(50));

            bool newAutoValue = GUILayout.Toggle(autoEnabled, "Auto " + label);
            if (newAutoValue != autoEnabled)
            {
                switch (key)
                {
                    case KeyCode.Q: Settings.autoQ = newAutoValue; break;
                    case KeyCode.W: Settings.autoW = newAutoValue; break;
                    case KeyCode.E: Settings.autoE = newAutoValue; break;
                    case KeyCode.R: Settings.autoR = newAutoValue; break;
                }
            }

            GUILayout.EndHorizontal();
        }

        public static void OnGUI()
        {
            if (guiVisible)
            {
                windowRect = GUI.Window(0, windowRect, (WindowFunction)DrawModWindow, "NPC Manager Mod");
            }
        }

        #region Auto Clicker Logic
        private static bool IsAutoEnabled(KeyCode key)
        {
            return key switch
            {
                KeyCode.Q => Settings.autoQ,
                KeyCode.W => Settings.autoW,
                KeyCode.E => Settings.autoE,
                KeyCode.R => Settings.autoR,
                _ => false
            };
        }

        private static float GetSkillCooldown(KeyCode key)
        {
            return key switch
            {
                KeyCode.Q => Settings.skillQ,
                KeyCode.W => Settings.skillW,
                KeyCode.E => Settings.skillE,
                KeyCode.R => Settings.skillR,
                _ => 5f
            };
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private static void SimulateKeyPress(KeyCode key)
        {
            byte vk = key switch
            {
                KeyCode.Q => 0x51,
                KeyCode.W => 0x57,
                KeyCode.E => 0x45,
                KeyCode.R => 0x52,
                _ => 0
            };

            if (vk != 0)
            {
                keybd_event(vk, 0, 0, 0);
                keybd_event(vk, 0, 2, 0);
            }
        }
        #endregion
    }
}