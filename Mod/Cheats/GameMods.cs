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
    }
}
