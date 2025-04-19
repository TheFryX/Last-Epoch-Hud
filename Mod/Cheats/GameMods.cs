using UnityEngine;
//using Mod.Utils;
using Mod.Game;
using System.Runtime.Serialization;
using ObjectManager = Mod.Game.ObjectManager;
using Il2Cpp;
using MelonLoader;
using UnityEngine.SceneManagement;

namespace Mod.Cheats
{
    internal static class GameMods
    {
        internal static bool someCondition = true;

        public static void FogRemover(bool areaChanged = true)
        {
            if (someCondition && areaChanged)
            {
                // Iterate through all loaded scenes
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (scene.isLoaded)
                    {
                        // Get all root objects in the scene
                        var rootObjects = scene.GetRootGameObjects();
                        foreach (var rootObject in rootObjects)
                        {
                            var lights = rootObject.GetComponentsInChildren<HxVolumetricLight>(true);
                            foreach (var light in lights)
                            {
                                if (light.gameObject.name == "Directional Light" ||
                                    light.gameObject.name == "Directional_DummyFogLight" && light.gameObject.activeSelf)
                                {
                                    if (light.dirty)
                                    {
                                        light.dirty = false;
                                        someCondition = false;
                                        MelonLogger.Msg(
                                            $"Found fog light: {light.gameObject.name}, " +
                                            $"rootObj: {rootObject.name}, " +
                                            $"scene: {scene.name}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                someCondition = false;
                //MelonLogger.Msg("No fog light found");
            }
        }

        internal static bool someCondition2 = true;
        public static void playerLantern(bool areaChanged = true)
        {
            //if (someCondition2 && areaChanged)
            //{
            //    var player = ObjectManager.GetLocalPlayer();
            //    if (player == null) return;

            //    var lights = player.GetComponent<Light>();
            //    if (someCondition2 && lights != null)
            //    {
            //        MelonLogger.Msg("Player light component found");
            //        lights.intensity = 3f;
            //        lights.range = 35f;
            //        lights.enabled = true;
            //    }
            //    else if (!someCondition2 && lights != null)
            //    {
            //        MelonLogger.Msg("Player light component not found");
            //        lights.intensity = 1f;
            //        lights.range = 12f;
            //        lights.enabled = false;
            //    }
            //    someCondition2 = false;
            //    MelonLogger.Msg("Player light component not found2");
            //}
            //else
            //{
            //    someCondition = false;
            //    MelonLogger.Msg("Player light component not found3");
            //}
        }
    }
}
