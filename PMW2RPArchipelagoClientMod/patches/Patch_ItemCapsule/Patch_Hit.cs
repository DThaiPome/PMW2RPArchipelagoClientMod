using Il2Cpp;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.services.items.v2.location.locations;

namespace PMW2RPArchipelagoClientMod.patches.Patch_ItemCapsule
{
    [HarmonyPatch(typeof(ItemCapsule), "Hit", [])]
    public class Patch_Hit
    {
        private static void Prefix(ItemCapsule __instance)
        {
            CollectCapsuleLocation.ClearCapsuleCollected(ServiceFactory.Locations, __instance.m_capsuleId);
        }
    }
}
