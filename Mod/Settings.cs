using System;
using System.Collections.Generic;
using System.IO;
using Il2Cpp;
using Newtonsoft.Json;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace Mod
{
    internal static class Settings
    {
        public static readonly string SETTINGS_DIR = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NPC_Manager_Mod");

        public static readonly string SETTINGS_FILE = Path.Combine(SETTINGS_DIR, "mod_settings.json");

        // UI States
        public static bool npcDrawingsDropdown = false;
        public static bool npcClassificationsDropdown = false;
        public static bool itemDrawingsDropdown = false;
        public static bool gamePatchesDropdown = false;
        public static bool riskyOptionsDropdown = false;
        public static bool skillsDropdown = false;
        public static bool autoPotDistanceDropdown = false;

        // Game Settings
        public static bool mapHack = true;
        public static float drawDistance = 100.0f;
        public static float autoHealthPotion = 50.0f;
        public static float timeScale = 1.0f;
        public static bool useAutoPot = true;
        public static bool useLootFilter = true;
        public static bool removeFog = true;
        public static bool cameraZoomUnlock = true;
        public static bool minimapZoomUnlock = true;
        public static bool playerLantern = true;
        public static bool useAnyWaypoint = false;
        public static bool pickupCrafting = false;

        // Skill Settings
        public static float skillQ = 5.0f;
        public static float skillW = 6.0f;
        public static float skillE = 5.0f;
        public static float skillR = 17.0f;
        public static bool autoQ = false;
        public static bool autoW = false;
        public static bool autoE = false;
        public static bool autoR = false;

        // Window Settings
        public static float windowX = 164.0f;
        public static float windowY = 488.0f;
        public static float windowWidth = 469.0f;
        public static float windowHeight = 562.0f;

        // NPC Configuration
        public static Dictionary<string, bool> npcClassifications = new Dictionary<string, bool>
        {
            { "Normal", false }, { "Magic", true }, { "Rare", true }, { "Boss", true }
        };

        public static Dictionary<string, bool> npcDrawings = new Dictionary<string, bool>
        {
            { "Good", false }, { "Evil", true }, { "Barrel", false },
            { "HostileNeutral", true }, { "FriendlyNeutral", true }, { "SummonedCorpse", true }
        };

        // Item Drawings
        public static Dictionary<string, bool> itemDrawings = new Dictionary<string, bool>
        {
            { "Magic", true }, { "Common", false }, { "Unique", true },
            { "Legendary", true }, { "Rare", true }, { "Set", true },
            { "Exalted", true }, { "Gold Piles", false }
        };

        static Settings()
        {
            InitializeDirectory();
            LoadSettings();
        }

        private static void InitializeDirectory()
        {
            if (!Directory.Exists(SETTINGS_DIR))
            {
                Directory.CreateDirectory(SETTINGS_DIR);
                Debug.Log("[NPC Mod] Config directory created: " + SETTINGS_DIR);
            }
        }

        public static void SaveSettings()
        {
            try
            {
                var settingsData = new
                {
                    // UI States
                    npcDrawingsDropdown,
                    npcClassificationsDropdown,
                    itemDrawingsDropdown,
                    gamePatchesDropdown,
                    riskyOptionsDropdown,
                    skillsDropdown,
                    autoPotDistanceDropdown,

                    // Game Settings
                    mapHack,
                    drawDistance,
                    autoHealthPotion,
                    timeScale,
                    useAutoPot,
                    useLootFilter,
                    removeFog,
                    cameraZoomUnlock,
                    minimapZoomUnlock,
                    playerLantern,
                    useAnyWaypoint,
                    pickupCrafting,

                    // Skills
                    skillQ,
                    skillW,
                    skillE,
                    skillR,
                    autoQ,
                    autoW,
                    autoE,
                    autoR,

                    // Window
                    windowX,
                    windowY,
                    windowWidth,
                    windowHeight,

                    // NPC & Items
                    npcClassifications,
                    npcDrawings,
                    itemDrawings
                };

                string json = JsonConvert.SerializeObject(settingsData, Formatting.Indented);
                File.WriteAllText(SETTINGS_FILE, json);
                Debug.Log("[NPC Mod] Settings saved successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError("[NPC Mod] Save error: " + ex.Message);
            }
        }

        public static void LoadSettings()
        {
            try
            {
                if (!File.Exists(SETTINGS_FILE))
                {
                    Debug.Log("[NPC Mod] No settings file found. Using defaults.");
                    return;
                }

                string json = File.ReadAllText(SETTINGS_FILE);
                dynamic data = JsonConvert.DeserializeObject(json);

                // Load basic settings with error handling
                npcDrawingsDropdown = GetSafeValue(data, "npcDrawingsDropdown", npcDrawingsDropdown);
                npcClassificationsDropdown = GetSafeValue(data, "npcClassificationsDropdown", npcClassificationsDropdown);
                itemDrawingsDropdown = GetSafeValue(data, "itemDrawingsDropdown", itemDrawingsDropdown);
                gamePatchesDropdown = GetSafeValue(data, "gamePatchesDropdown", gamePatchesDropdown);
                riskyOptionsDropdown = GetSafeValue(data, "riskyOptionsDropdown", riskyOptionsDropdown);
                skillsDropdown = GetSafeValue(data, "skillsDropdown", skillsDropdown);
                autoPotDistanceDropdown = GetSafeValue(data, "autoPotDistanceDropdown", autoPotDistanceDropdown);

                mapHack = GetSafeValue(data, "mapHack", mapHack);
                drawDistance = GetSafeValue(data, "drawDistance", drawDistance);
                autoHealthPotion = GetSafeValue(data, "autoHealthPotion", autoHealthPotion);
                timeScale = GetSafeValue(data, "timeScale", timeScale);
                useAutoPot = GetSafeValue(data, "useAutoPot", useAutoPot);
                useLootFilter = GetSafeValue(data, "useLootFilter", useLootFilter);
                removeFog = GetSafeValue(data, "removeFog", removeFog);
                cameraZoomUnlock = GetSafeValue(data, "cameraZoomUnlock", cameraZoomUnlock);
                minimapZoomUnlock = GetSafeValue(data, "minimapZoomUnlock", minimapZoomUnlock);
                playerLantern = GetSafeValue(data, "playerLantern", playerLantern);
                useAnyWaypoint = GetSafeValue(data, "useAnyWaypoint", useAnyWaypoint);
                pickupCrafting = GetSafeValue(data, "pickupCrafting", pickupCrafting);

                skillQ = GetSafeValue(data, "skillQ", skillQ);
                skillW = GetSafeValue(data, "skillW", skillW);
                skillE = GetSafeValue(data, "skillE", skillE);
                skillR = GetSafeValue(data, "skillR", skillR);
                autoQ = GetSafeValue(data, "autoQ", autoQ);
                autoW = GetSafeValue(data, "autoW", autoW);
                autoE = GetSafeValue(data, "autoE", autoE);
                autoR = GetSafeValue(data, "autoR", autoR);

                windowX = GetSafeValue(data, "windowX", windowX);
                windowY = GetSafeValue(data, "windowY", windowY);
                windowWidth = GetSafeValue(data, "windowWidth", windowWidth);
                windowHeight = GetSafeValue(data, "windowHeight", windowHeight);

                // Load dictionaries with key correction
                LoadDictionaryWithFix(data, "npcClassifications", npcClassifications);
                LoadDictionaryWithFix(data, "npcDrawings", npcDrawings);
                LoadDictionaryWithFix(data, "itemDrawings", itemDrawings);

                Debug.Log("[NPC Mod] Settings loaded successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError("[NPC Mod] Load error: " + ex.Message);
                ResetToDefaults();
            }
        }

        private static T GetSafeValue<T>(dynamic data, string key, T defaultValue)
        {
            try
            {
                if (data[key] != null)
                    return (T)Convert.ChangeType(data[key], typeof(T));
            }
            catch
            {
                Debug.LogWarning($"[NPC Mod] Error loading {key}, using default value");
            }
            return defaultValue;
        }

        private static void LoadDictionaryWithFix(dynamic data, string key, Dictionary<string, bool> targetDict)
        {
            try
            {
                if (data[key] != null)
                {
                    targetDict.Clear();
                    foreach (var entry in data[key])
                    {
                        string fixedKey = entry.Name.ToString()
                            .Replace("Bare", "Rare")
                            .Replace("SummonedCorps=", "SummonedCorpse");
                        targetDict[fixedKey] = entry.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[NPC Mod] Error loading dictionary {key}: {ex.Message}");
                ResetDictionary(targetDict);
            }
        }

        private static void ResetDictionary(Dictionary<string, bool> targetDict)
        {
            try
            {
                targetDict.Clear();
                var defaults = typeof(Settings).GetField(targetDict == npcClassifications ? "npcClassifications" :
                                                        targetDict == npcDrawings ? "npcDrawings" : "itemDrawings",
                                                        BindingFlags.Public | BindingFlags.Static)
                                             .GetValue(null) as Dictionary<string, bool>;

                foreach (var kvp in defaults)
                {
                    targetDict[kvp.Key] = kvp.Value;
                }
            }
            catch { }
        }

        private static void ResetToDefaults()
        {
            try
            {
                typeof(Settings).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => !f.IsLiteral)
                    .ToList()
                    .ForEach(field =>
                    {
                        if (field.FieldType == typeof(Dictionary<string, bool>))
                        {
                            ResetDictionary(field.GetValue(null) as Dictionary<string, bool>);
                        }
                        else if (field.Name != "SETTINGS_DIR" && field.Name != "SETTINGS_FILE")
                        {
                            var defaultValue = typeof(Settings).GetField(field.Name, BindingFlags.Public | BindingFlags.Static)
                                                             .GetValue(null);
                            field.SetValue(null, defaultValue);
                        }
                    });
            }
            catch { }
        }

        #region Drawing Helpers
        public static bool DrawGoldPiles() => itemDrawings.TryGetValue("Gold Piles", out bool value) && value;

        public static bool ShouldDrawItemRarity(string rarity) =>
            !string.IsNullOrEmpty(rarity) && itemDrawings.TryGetValue(rarity, out bool value) && value;

        public static bool ShouldDrawNPCAlignment(string alignment) =>
            !string.IsNullOrEmpty(alignment) && npcDrawings.TryGetValue(alignment, out bool value) && value;

        public static bool ShouldDrawNPCClassification(string classification) =>
            !string.IsNullOrEmpty(classification) && npcClassifications.TryGetValue(classification, out bool value) && value;

        public static bool ShouldDrawNPCClassification(DisplayActorClass classification) =>
            ShouldDrawNPCClassification(classification.ToString());
        #endregion
    }
}