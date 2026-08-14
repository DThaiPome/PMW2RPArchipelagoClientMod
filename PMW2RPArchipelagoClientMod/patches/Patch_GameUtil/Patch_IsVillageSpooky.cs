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
            __result = ServiceFactory.Unlocks.Stages.GetValueOrDefault(EWorldStage.Stage6_4, false)
                && PACWSaveData.GetStageFlag((int)EWorldStage.Stage6_4) != EStageFlag.Clear;
            return false;
        }
    }
}
