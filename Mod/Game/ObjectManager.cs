using UnityEngine;

namespace Mod.Game
{
    internal class ObjectManager
    {
        private static GameObject? localPlayer;

        public static void AttemptToFindPlayer()
        {
            localPlayer = GameObject.Find("MainPlayer(Clone)");
            if (localPlayer == null)
            {
                localPlayer = GameObject.Find("Local Player(Clone)");
            }
        }

        public static void OnSceneLoaded()
        {
            AttemptToFindPlayer();
        }

        public static bool HasPlayer()
        {
            if (localPlayer == null)
            {
                AttemptToFindPlayer();
            }

            return localPlayer != null;
        }

        public static GameObject? GetLocalPlayer()
        {
            if (localPlayer == null)
            {
                AttemptToFindPlayer();
            }

            return localPlayer;
        }
    }
}
