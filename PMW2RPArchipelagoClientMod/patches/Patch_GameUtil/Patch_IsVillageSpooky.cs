using Il2Cpp;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.services.items.item.items;

namespace PMW2RPArchipelagoClientMod.patches.Patch_GameUtil
{
    [HarmonyPatch(typeof(GameUtil), "IsVillageSpooky", [])]
    public class Patch_IsVillageSpooky
    {
        private static bool Prefix(ref bool __result)
        {
            if (!ServiceFactory.GameSaveDataService.SaveOperationsAllowed)
            {
                return true;
            }
            __result = GoldenFruitItem.AreAllGoldenFruitsUnlocked(ServiceFactory.Unlocks)
                && ServiceFactory.GameSaveDataService.IsSpookyUnlockedOrPlayed();
            return false;
        }
    }
}
