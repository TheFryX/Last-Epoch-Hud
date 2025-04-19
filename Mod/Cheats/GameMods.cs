using UnityEngine;
using ObjectManager = Mod.Game.ObjectManager;
using Il2Cpp;
using MelonLoader;
using UnityEngine.SceneManagement;

namespace Mod.Cheats
{
    internal static class GameMods
    {
        public static void FogRemover(bool areaChanged = true)
        {
            if (Settings.removeFog && areaChanged)
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
                                        MelonLogger.Msg("[Mod] patched fog");
                                        //MelonLogger.Msg(
                                        //    $"Found fog light: {light.gameObject.name}, " +
                                        //    $"rootObj: {rootObject.name}, " +
                                        //    $"scene: {scene.name}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void playerLantern(bool areaChanged = true)
        {
            if (Settings.playerLantern && areaChanged)
            {
                var player = ObjectManager.GetLocalPlayer();
                if (player == null) return;

                var lights = player.GetComponentsInChildren<Light>(true);

                foreach (var light in lights)
                {
                    if (light.gameObject.name == "Front Spot Light")
                    {
                        if (!light.enabled)
                        {
                            light.intensity = 3f;
                            light.range = 35f;
                            light.enabled = true;
                            MelonLogger.Msg(
                                $"Found player light: {light.gameObject.name}");
                            return;
                        }
                        else
                        {
                            MelonLogger.Msg("No deactivated player light found");

                            //light.intensity = 1f;
                            //light.range = 12f;
                            //light.enabled = false;
                            //someCondition2 = false;
                            MelonLogger.Msg(
                                $"Found light: {light.gameObject.name}");
                        }
                        MelonLogger.Msg("No player light found");
                    }
                    else
                        MelonLogger.Msg("No player light object found");
                }
            }
        }
    }
}
