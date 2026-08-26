using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_ItemCapsule
{
    [HarmonyPatch(typeof(ItemCapsule), "Hit", [])]
    public class Patch_Hit
    {
        private static void Prefix(ItemCapsule __instance)
        {
            ServiceFactory.Locations.CollectCapsule(__instance.m_capsuleId);
        }
    }
}
