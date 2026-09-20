using HarmonyLib;
using Il2Cpp;
using UnityEngine.Events;
using UnityEngine;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.util;

namespace PMW2RPArchipelagoClientMod.patches.Patch_ObjMaker_PopItem
{
    [HarmonyPatch(typeof(ObjMaker.PopItem), "CreatePopItem")]
    public class Patch_CreatePopItem
    {
        private static bool Prefix(Transform parent, UnityAction<ItemBase> onCreateItem, bool ignoreRestore, ObjMaker.PopItem __instance, ref GameObject __result)
        {
            if (!FruitsUtil.MatchItemSwitchUnlocked(__instance.itemKind, ServiceFactory.Unlocks))
            {
                __result = null;
                return false;
            }
            return true;
        }
    }
}
