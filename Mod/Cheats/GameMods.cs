using UnityEngine;
//using Mod.Utils;
using Mod.Game;
using System.Runtime.Serialization;
using ObjectManager = Mod.Game.ObjectManager;
using Il2Cpp;
using MelonLoader;

namespace Mod.Cheats
{
    internal static class GameMods
    {
        internal static bool someCondition = true;

        public static void FogRemover(bool areaChanged = true)
        {
            if (someCondition && areaChanged)
            {
                var allLights = GameObject.FindObjectsOfType<HxVolumetricLight>();
                foreach (var light in allLights)
                {
                    if (light.gameObject.name == "Directional Light" || 
                        light.gameObject.name == "Directional_DummyFogLight" && light.gameObject.activeInHierarchy)
                    {
                        if (light.dirty)
                        {
                            light.dirty = false;
                            someCondition = false;
                            MelonLogger.Msg($"Found fog light: {light.gameObject.name}");
                        }
                    }
                    else
                    {
                        MelonLogger.Msg("No fog light found");
                    }
                }
            }
        }
    }
}
