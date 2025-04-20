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
            //todo: getting kicked due to idle can cause this to stop triggering somehow
            //todo: alternatively we may need to do more checks then just sceneInit (x2)
            if (Settings.removeFog && areaChanged)
            {
                // Iterate through all loaded scenes
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (scene.isLoaded || scene.isSubScene)
                    {
                        MelonLogger.Msg($"Patching fog for: {scene.name}");
                        // Get all root objects in the scene
                        var rootObjects = scene.GetRootGameObjects();
                        foreach (var rootObject in rootObjects)
                        {
                            var lights = rootObject.GetComponentsInChildren<HxVolumetricLight>(true);
                            foreach (var light in lights)
                            {
                                //if (light.gameObject.name == "Directional Light" ||
                                //    light.gameObject.name == "HXObject" ||
                                //    light.gameObject.name == "DummyLight" ||
                                //    light.gameObject.name == "SecondaryDirectionalLight_Dummy" ||
                                //    light.gameObject.name == "Directional_DummyFogLight")
                                //{
                                if (light.dirty)
                                    {
                                        light.dirty = false;
                                        //MelonLogger.Msg(
                                        //    $"Found fog light: {light.gameObject.name}, " +
                                        //    $"rootObj: {rootObject.name}, " +
                                        //    $"scene: {scene.name}");
                                    }
                                //}
                            }
                        }
                    }
                }
            }
        }

        #region player lantern working but not working
        public static void playerLantern(bool areaChanged = true)
        {
            //todo: investigate Local Player(Clone)/Player Lights<ConstantRotation>
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
                                $"Found & patched player light: {light.gameObject.name}");
                            return;
                        }
                        else
                        {
                            //MelonLogger.Msg("No deactivated player light found");

                            //light.intensity = 1f;
                            //light.range = 12f;
                            //light.enabled = false;
                            //someCondition2 = false;
                            //MelonLogger.Msg(
                            //    $"Found light: {light.gameObject.name}");
                        }
                        //MelonLogger.Msg("No player light found");
                    }
                    //else
                        //MelonLogger.Msg("No player light object found");
                }
            }
        }
        #endregion

        #region coroutine to try and repeat patch lantern. why is it failing? melon coroutines?
        //private static Coroutine? _lightCheckCoroutine;
        //public static void playerLantern(bool areaChanged = true)
        //{
        //    if (Settings.playerLantern && areaChanged)
        //    {
        //        if (_lightCheckCoroutine != null)
        //        {
        //            // Stop the existing coroutine if it's already running  
        //            MelonCoroutines.Stop(_lightCheckCoroutine);
        //        }

        //        // Start a new coroutine to periodically check the light  
        //        _lightCheckCoroutine = (Coroutine)MelonCoroutines.Start(CheckAndPatchPlayerLight());
        //    }
        //    else if (!Settings.playerLantern && _lightCheckCoroutine != null)
        //    {
        //        // Stop the coroutine if the mod is disabled  
        //        MelonCoroutines.Stop(_lightCheckCoroutine);
        //        _lightCheckCoroutine = null;
        //    }
        //}

        //private static IEnumerator CheckAndPatchPlayerLight()
        //{
        //    while (true)
        //    {
        //        var player = ObjectManager.GetLocalPlayer();
        //        if (player == null) yield break;

        //        var lights = player.GetComponentsInChildren<Light>(true);

        //        foreach (var light in lights)
        //        {
        //            if (light.gameObject.name == "Front Spot Light")
        //            {
        //                if (!light.enabled)
        //                {
        //                    light.intensity = 3f;
        //                    light.range = 35f;
        //                    light.enabled = true;
        //                    MelonLogger.Msg($"[Mod] Found & patched player light: {light.gameObject.name}");
        //                }
        //            }
        //        }

        //        // Wait for 10 seconds before checking again  
        //        yield return new WaitForSeconds(10f);
        //    }
        //}
        #endregion
    }
}
