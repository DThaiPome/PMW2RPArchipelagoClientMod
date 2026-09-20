using Il2Cpp;
using HarmonyLib;
using UnityEngine;
using PMW2RPArchipelagoClientMod.util;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_EnemyBase
{
    [HarmonyPatch(typeof(EnemyBase), "ItemDrop", [typeof(ObjMaker.PopItem), typeof(bool)])]
    public class Patch_ItemDrop_1
    {
        private static bool Prefix(ObjMaker.PopItem info, bool isForceStalk, ref GameObject __result)
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
