using Il2Cpp;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;

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
            __result = ServiceFactory.Unlocks.AreAllGoldenFruitsUnlocked()
                && ServiceFactory.GameSaveDataService.IsSpookyUnlockedOrPlayed();
            return false;
        }
    }
}
