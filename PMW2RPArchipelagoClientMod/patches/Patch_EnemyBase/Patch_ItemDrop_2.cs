using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.util;
using UnityEngine;
using static Il2Cpp.EnemyBase;

namespace PMW2RPArchipelagoClientMod.patches.Patch_EnemyBase
{
    [HarmonyPatch(typeof(EnemyBase), "ItemDrop", [typeof(DropItemInfo), typeof(bool)])]
    public class Patch_ItemDrop_2
    {
        private static bool Prefix(DropItemInfo info, bool isForceStalk, ref GameObject __result)
        {
            if (!FruitsUtil.MatchItemSwitchUnlocked(info.itemKind, ServiceFactory.Unlocks))
            {
                __result = null;
                return false;
            }
            return true;
        }
    }
}
