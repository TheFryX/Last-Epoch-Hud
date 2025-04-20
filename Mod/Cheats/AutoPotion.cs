using Il2Cpp;
using Mod.Game;

namespace Mod.Cheats
{
    internal class AutoPotion
    {
        // Debounce the potion usage
        private static DateTime lastUse = DateTime.MinValue;

        public static void UseHealthPotion()
        {
            if (Settings.useAutoPot == false) return;
            if (DateTime.Now - lastUse < TimeSpan.FromSeconds(1)) return;

            lastUse = DateTime.Now;

            var localPlayer = ObjectManager.GetLocalPlayer();
            if (localPlayer == null) return;

            var healthPotion = localPlayer.GetComponent<LocalPlayer>();
            if (healthPotion != null)
            {
                var reapercheck = localPlayer.GetComponentInChildren<ChangeHealthMaterialDuringLifetime>();
                if (reapercheck != null && reapercheck.materialToChangeTo == UIGlobeHealth.AlternateMaterial.ReaperForm)
                {
                    //MelonLogger.Msg("Reaper is active, not using health potion.");
                    return;
                }
                healthPotion.PotionKeyPressed();
            }
        }

        public static void OnUpdate()
        {
            if (!ObjectManager.HasPlayer()) return;

            var localPlayer = ObjectManager.GetLocalPlayer();

            if (localPlayer == null) return;

            var playerHealth = localPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.getHealthPercent() * 100 <= Settings.autoHealthPotion)
            {
                UseHealthPotion();
            }
        }
    }
}
