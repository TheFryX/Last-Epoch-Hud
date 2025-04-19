using Il2Cpp;
using Il2CppItemFiltering;
using Mod.Game;
using UnityEngine;

namespace Mod.Cheats.ESP
{
    internal class Items
    {
        public static void GatherItems()
        {
            if (GroundItemVisuals.all == null) return;

            foreach (var item in GroundItemVisuals.all._list)
            {
                // Ensure the item is active in the scene  
                if (item?.gameObject == null || !item.gameObject.activeInHierarchy) return;

                var localPlayer = ObjectManager.GetLocalPlayer();
                if (localPlayer == null || localPlayer.transform == null) return;

                if (Vector3.Distance(localPlayer.transform.position, item.transform.position) > Settings.drawDistance) continue;

                Rule.RuleOutcome filter = ItemFiltering.Match(item.itemData, null, null, 0);

                if (Settings.useLootFilter && filter == Rule.RuleOutcome.HIDE) continue;

                var rarity = item.groundItemRarityVisuals?.name;

                // Ensure rarity is not null before calling ShouldDrawItemRarity  
                if (rarity == null || !Settings.ShouldDrawItemRarity(rarity))
                {
                    continue;
                }

                var color = Drawing.ItemRarityToColor(rarity);

                ESP.AddLine(localPlayer.transform.position, item.transform.position, color);
                ESP.AddString(item.itemData.FullName, item.transform.position, color);
            }
        }

        public static void OnUpdate()
        {
            if (ObjectManager.HasPlayer())
            {
                GatherItems();
            }
        }
    }
}
