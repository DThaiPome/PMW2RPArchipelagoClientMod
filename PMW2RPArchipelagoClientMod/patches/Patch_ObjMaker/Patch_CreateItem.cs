using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2Cpp;
using UnityEngine.Events;
using UnityEngine;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.util;

namespace PMW2RPArchipelagoClientMod.patches.Patch_ObjMaker
{
    [HarmonyPatch(typeof(ObjMaker), "CreateItem")]
    public class Patch_CreateItem
    {
        private static bool Prefix(Transform parent, EItem kind, UnityAction<ItemBase> onCreate, bool ignoreRestore, int itemSaveId, ref GameObject __result)
        {
            if (!FruitsUtil.MatchItemSwitchUnlocked(kind, ServiceFactory.Unlocks))
            {
                __result = null;
                return false;
            }
            return true;
        }
    }
}
