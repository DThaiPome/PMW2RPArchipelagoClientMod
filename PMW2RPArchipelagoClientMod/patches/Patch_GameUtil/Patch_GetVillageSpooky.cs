using Il2Cpp;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;

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
            if (ServiceFactory.Unlocks.IsSpookyUnlocked()
                && ServiceFactory.GameSaveDataService.GetStageFlag(EWorldStage.Stage6_4) != EStageFlag.Clear)
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
