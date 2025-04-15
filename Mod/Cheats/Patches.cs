using Il2CppInterop.Runtime;
using System.Runtime.Serialization;
using UnityEngine;
using Mod.Game;
//using static UnityEngine.Physics;
using UnityEngine.UI;
using Harmony;
using HarmonyLib;
using Il2Cpp;
using UnityEngine.Playables;
using ObjectManager = Mod.Game.ObjectManager;
using HarmonyPatch = HarmonyLib.HarmonyPatch;
using MelonLoader;
using HarmonyPostfix = HarmonyLib.HarmonyPostfix;
using HarmonyPrefix = HarmonyLib.HarmonyPrefix;
using System.Reflection;
using AccessTools = HarmonyLib.AccessTools;
//using static Il2Cpp.UIChat;
using Il2CppTMPro;
using Il2CppDMM;

namespace Mod.Cheats.Patches
{
    public class MapIconPatch
    {
        private bool isInitialized = false;

        public DMMapIcon? InitializeDMMapIcon(string name)
        {
            // todo: probably re-do this to just pass the found actor to avoid the string lookup?
            // todo: how will we track when theyre dead to remove icon?
            // i think corpses despawn so maybe okay to just leave it?
            // otherwise can make an async task that does bg check for alive status
            if (isInitialized) return null;
            isInitialized = true;

            GameObject npcObj = GameObject.Find(name);
            if (npcObj != null)
            {
                return npcObj.AddComponent<DMMapIcon>();
            }
            else
                return null;
        }
        public static void InitializeDMMapIcon(GameObject actor)
        {
            if (actor.GetComponent<DMMapIconLabel>() == null)
            {
                DMMapIconLabel mapIcon = actor.AddComponent<DMMapIconLabel>();

                // Modify the RectTransform to adjust the size
                RectTransform rt = mapIcon.GetComponent<RectTransform>();
                if (rt != null)
                {
                    // Shrink the icon size to 20x20 pixels (or adjust as needed)
                    rt.sizeDelta = new Vector2(20, 20);
                }

                mapIcon.text = "*";
                // Change the color of the DMMapIcon (derived from Image)
                mapIcon.color = Color.red;  // Change to any desired UnityEngine.Color
            }
        }
    }

    [HarmonyPatch]
    internal class HarmonyPatches
    {
        // todo implement patches to guard accident accidental self reports of modifying the game
        // patch out the bug report button, and for redundancy the SendBugToServer() function
        // il2Cppscripts->il2cpp->BugReportWriter->SendBugToServer
        // il2CppLE->il2Cpp->UIBase->OpenBugReportPanel()
        // il2CppLE->il2Cpp->CharacterSelectPanelUI->OpenBugReport()
        // il2CppLE->il2Cpp->Il2CppLE.Telemetry->ClientSessionAnalytics->BugReported()
        // il2CppLE->il2Cpp->Il2CppLE.Telemetry->ClientSessionAnalytics->OnBugReported()
        // il2CppLE->il2Cpp->Il2CppLE.Telemetry->ISessionAnalytics->OnBugReported()
        // il2CppLE->il2Cpp->il2CppLELE.Utility->ClickUp->SubmitNewBugReport()

        // probably should patch out the crash reporter just incase
        // UnityEngine.CrashReportingModule->UnityEngine.CrashReportHandler->CrashReportHandler()
        // UnityEngine.CoreModule->UnityEngine->CrashReport()

        // other possibly related hooks to patch out
        // il2CppLE->il2Cpp->Il2CppLE.Services->ChatManager->_LogErrorAndSendTelemetryEvent_d__36
        // il2CppLE->il2Cpp->Il2CppPlayFab->Il2CppPlayFab->PlayFabEventsAPI->WriteTelemetryEvents()

        [HarmonyPatch(typeof(DMMapIcon), "UpdateIcons")]
        public class DMMapIconHooks
        {
            private static bool isFriendlyDotFound = false;
            private static Image? friendlyDotImage = null;

            public static Image? FriendlyDotImage => friendlyDotImage;

            private static void Postfix(DMMapIcon __instance)
            {
                if (isFriendlyDotFound) return;

                if (__instance != null)
                {
                    Image? imageComponent = __instance.GetComponent<Image>();
                    if (imageComponent != null && __instance.name == "friendly-dot")
                    {
                        friendlyDotImage = imageComponent;
                        isFriendlyDotFound = true;

                        MelonLogger.Msg($"[Mod] Found 'friendly-dot' with Image component. Storing reference.");
                    }
                }
                else
                {
                    MelonLogger.Msg("[Mod] DMMapIcon instance is null.");
                }
            }
        }
    }
}
