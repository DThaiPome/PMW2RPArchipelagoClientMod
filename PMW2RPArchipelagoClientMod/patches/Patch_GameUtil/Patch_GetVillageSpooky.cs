using Il2Cpp;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items;

namespace PMW2RPArchipelagoClientMod.patches.Patch_GameUtil
{
    [HarmonyPatch(typeof(GameUtil), "GetVillageSpooky", [])]
    public class Patch_GetVillageSpooky
    {
        private static bool Prefix(ref StageInfo __result)
        {
            if (!ServiceFactory.GameSaveDataService.SaveOperationsAllowed)
            {
                return true;
            }
            if (GoldenFruitItem.AreAllGoldenFruitsUnlocked(ServiceFactory.Unlocks)
                && ServiceFactory.GameSaveDataService.IsSpookyUnlockedOrPlayed())
            {
                __result = MasterData.GetStage(EArea.Area6, 4);
            }
            else
            {
                __result = MasterData.GetStage(EWorldStage.PacVillage);
            }
            return false;
        }
    }
}
