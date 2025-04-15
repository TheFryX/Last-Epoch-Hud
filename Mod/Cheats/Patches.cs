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

namespace Mod.Cheats.Patches
{
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
    }
}
