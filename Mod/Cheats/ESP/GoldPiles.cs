using Il2Cpp;
using Mod.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Mod.Cheats.ESP
{
    internal class GoldPiles
    {
        public static void GatherGoldPiles()
        {
            if (!Settings.DrawGoldPiles()) return;
            if (GroundGoldVisuals.all == null) return;

            foreach (var item in GroundGoldVisuals.all._list)
            {
                if (item?.gameObject == null || !item.gameObject.activeInHierarchy) continue;

                var localPlayer = ObjectManager.GetLocalPlayer();
                if (localPlayer?.transform == null) continue;

                if (Vector3.Distance(localPlayer.transform.position, item.transform.position) > Settings.drawDistance) continue;

                ESP.AddLine(localPlayer.transform.position, item.transform.position, Color.white);
                ESP.AddString(item.goldValue.ToString() + " Gold", item.transform.position, Color.white);
            }
        }
        public static void OnUpdate()
        {
            if (ObjectManager.HasPlayer())
            {
                GatherGoldPiles();
            }
        }
    }
}
