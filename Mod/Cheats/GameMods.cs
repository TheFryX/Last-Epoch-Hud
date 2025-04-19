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
                // SceneObjects/Visuals/Lighting/
                var dirLighting = GameObject.FindAnyObjectByType<HxVolumetricLight>();

                if (dirLighting != null)
                {
                    if (dirLighting.dirty == true)
                        dirLighting.dirty = false;
                    someCondition = false;
                }
            }
        }
    }
}
