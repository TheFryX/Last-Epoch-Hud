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
    public class MapIconPatch : MonoBehaviour
    {
        private bool isInitialized = false;

        public void InitializeDMMapIcon(GameObject actor)
        {
            if (isInitialized) return;

            //var ourCont = actor.AddComponent<MinimapIconManager>();

            if (actor.GetComponent<DMMapIconManager>() == null)
            {
                //DMMapIconManager? mapIcon = actor.AddComponent<DMMapIconManager>();
                //BaseDMMapIcon? baseMapIcon = null;

                //if (mapIcon == null)
                //{
                //    MelonLogger.Msg($"[Mod] Failed to add DMMapIconManager to {actor.name}");
                //    return;
                //}
                //else
                //{
                //    mapIcon.Start();
                //    baseMapIcon = mapIcon.GetComponent<BaseDMMapIcon>();
                //}


                //mapIcon.icon = DMMapWorldIcon.iconType.arenaIcon;

                // Modify the RectTransform to adjust the size
                //RectTransform rt = mapIcon.GetComponent<RectTransform>();
                //if (rt != null)
                //{
                //    // Shrink the icon size to 20x20 pixels (or adjust as needed)
                //    rt.sizeDelta = new Vector2(20, 20);
                //}

                //mapIcon.img = HarmonyPatches.DMMapIconHooks.FriendlyDotImage;

                //mapIcon.text = "*";
                // Change the color of the DMMapIcon (derived from Image)
                //mapIcon.img.color = Color.red;  // Change to any desired UnityEngine.Color
            }
            isInitialized = true;
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
            //private static bool isFriendlyDotFound = false;
            //private static Image? friendlyDotImage = null;
            //private static Sprite? friendlyDotSprite = null;

            //public static Image? FriendlyDotImage => friendlyDotImage;
            //public static Sprite? FriendlyDotSprite => friendlyDotSprite;
            private static void Postfix(DMMapIcon __instance)
            {
                //if (isFriendlyDotFound) return;

                if (__instance != null)
                {
                    //MelonLogger.Msg($"[Mod] DMMapIcon instance: {__instance.name}");
                    //GameObject? gObject = __instance.gameObject;
                    //Image? imageComponent = gObject.GetComponent<Image>();
                    //Sprite? spriteComponent = gObject.GetComponent<Sprite>();
                    //if (spriteComponent == null && imageComponent != null)
                    //{
                    //    MelonLogger.Msg("[Mod] DMMapIcon sprite is null. Trying to find in children.");
                    //    spriteComponent = imageComponent.GetComponent<Sprite>();
                    //}
                    //if (imageComponent != null && spriteComponent != null && spriteComponent.name == "friendly-dot")
                    //{
                    //    friendlyDotImage = imageComponent;
                    //    friendlyDotSprite = spriteComponent;
                    //    isFriendlyDotFound = true;

                    //    MelonLogger.Msg($"[Mod] Found 'friendly-dot' with Image component. Storing reference.");
                    //}
                }
                else
                {
                    MelonLogger.Msg("[Mod] DMMapIcon.UpdateIcons instance is null.");
                }
            }
        }
        [HarmonyPatch(typeof(DMMapWorldIcon), "SetIcon")]
        public class DMMapWorldIconHooks
        {
            //private static void Prefix(DMMapWorldIcon __instance)
            //{
            //    if (__instance != null)
            //    {
            //        //MelonLogger.Msg($"[Mod] DMMapIconManager.SetIcon Prefix instance: {__instance.name}");
            //        MelonLogger.Msg($"[Mod] DMMapWorldIcon.SetIcon Prefix currentIcon: {__instance.currentIcon}");
            //        MelonLogger.Msg($"[Mod] DMMapWorldIcon.SetIcon Prefix IconType: {__instance.icon}");
            //    }
            //}
            private static void Postfix(DMMapWorldIcon __instance)
            {
                if (__instance != null)
                {
                    MelonLogger.Msg($"[Mod] DMMapIconManager.SetIcon Postfix instance: {__instance.name}");

                    //MelonLogger.Msg($"[Mod] DMMapWorldIcon.SetIcon Postfix currentIcon: {__instance.currentIcon}");
                    //MelonLogger.Msg($"[Mod] DMMapWorldIcon.SetIcon Postfix IconType: {__instance.icon}");
                }
            }
        }

        [HarmonyPatch(typeof(DMMapIconManager), "Start")]
        public class DMMapIconManagerHooks
        {
            // the flow seems to be start from DMMapIconManager.Start -> BaseDMMapIcon.initialise to create minion icons on map
            private static void Prefix(DMMapIconManager __instance)
            {
                if (__instance != null)
                {
                    MelonLogger.Msg($"[Mod] DMMapIconManager.Start Prefix instance: {__instance.name}");

                    //MelonLogger.Msg($"[Mod] DMMapIconManager.Start Prefix currentIcon: {__instance.currentIcon}");
                    //MelonLogger.Msg($"[Mod] DMMapIconManager.Start Prefix IconType: {__instance.icon}");
                }
            }
            private static void Postfix(DMMapIconManager __instance)
            {
                if (__instance != null)
                {
                    MelonLogger.Msg($"[Mod] DMMapIconManager.Start Postfix instance: {__instance.name}");

                    //MelonLogger.Msg($"[Mod] DMMapWorldIcon.Start Postfix currentIcon: {__instance.currentIcon}");
                    //MelonLogger.Msg($"[Mod] DMMapWorldIcon.Start Postfix IconType: {__instance.icon}");
                }
            }
        }

        //[HarmonyPatch(typeof(BaseDMMapIcon), "initialise")]
        //public class BaseDMMapIconInitHooks
        //{
        //    private static void Prefix(BaseDMMapIcon __instance)
        //    {
        //        if (__instance != null)
        //        {
        //            MelonLogger.Msg($"[Mod] BaseDMMapIcon.initialise Prefix instance: {__instance.name}");

        //            //MelonLogger.Msg($"[Mod] DMMapIconManager.initialise Prefix currentIcon: {__instance.currentIcon}");
        //            //MelonLogger.Msg($"[Mod] DMMapIconManager.initialise Prefix IconType: {__instance.icon}");
        //        }
        //    }
        //    private static void Postfix(BaseDMMapIcon __instance)
        //    {
        //        if (__instance != null)
        //        {
        //            MelonLogger.Msg($"[Mod] BaseDMMapIcon.initialise Postfix instance: {__instance.name}");

        //            //MelonLogger.Msg($"[Mod] DMMapWorldIcon.initialise Postfix currentIcon: {__instance.currentIcon}");
        //            //MelonLogger.Msg($"[Mod] DMMapWorldIcon.initialise Postfix IconType: {__instance.icon}");
        //        }
        //    }
        //}

        [HarmonyPatch(typeof(BaseDMMapIcon), nameof(BaseDMMapIcon.initialise))]
        [HarmonyPostfix]
        private static void initialisePostfix(BaseDMMapIcon __instance)
        {
            if (__instance == null) return;

            MelonLogger.Msg($"[Mod] BaseDMMapIcon.initialise Postfix: {__instance.name}");
        }

        // this one fires every frame, we should avoid hooking into it unless necessary
        //[HarmonyPatch(typeof(BaseDMMapIcon), nameof(BaseDMMapIcon.UpdateIconSprite))]
        //[HarmonyPostfix]
        //private static void UpdateIconSpritePostfix(BaseDMMapIcon __instance)
        //{
        //    if (__instance == null) return;

        //    //MelonLogger.Msg($"[Mod] BaseDMMapIcon.UpdateIconSprite: {__instance}");
        //}

        // this one has no names we can grab, even from the GO. unsure how useful it will be
        //[HarmonyPatch(typeof(BaseDMMapIcon), nameof(BaseDMMapIcon.UpdateIcons))]
        //[HarmonyPostfix]
        //private static void UpdateIconsPostfix(BaseDMMapIcon __instance)
        //{
        //    //if (__instance == null) return;

        //    //MelonLogger.Msg($"[Mod] BaseDMMapIcon.UpdateIcons: {__instance.name}");
        //}
    }
}
