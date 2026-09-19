using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;
using UnityEngine;
using PMW2RPArchipelagoClientMod.util;

namespace PMW2RPArchipelagoClientMod.patches.Patch_ItemBase
{
    [HarmonyPatch(typeof(ItemBase), "Awake")]
    public class Patch_Awake
    {
        private static bool Prefix(ItemBase __instance)
        {
            if (!FruitsUtil.MatchItemSwitchUnlocked(__instance.ItemKind, ServiceFactory.Unlocks))
            {
                GameObject.Destroy(__instance.gameObject);
                return false;
            }
            return true;
        }
    }
}
