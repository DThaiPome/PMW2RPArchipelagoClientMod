using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_GimmickGoStage
{
    [HarmonyPatch(typeof(GimmickGoStage), "get_IsExecOK", [])]
    public class Patch_IsExecOK
    {
        private static void Postfix(GimmickGoStage __instance, ref bool __result)
        {
            if (!(ServiceFactory.APConnectionService.IsLevelRando ?? true))
            {
                return;
            }
            SS_GoStageRoot stageObj = __instance.m_rootScript;
            if (stageObj == null || stageObj.Kind == SS_GoStageRoot.EKind.PacVillage || stageObj.Kind == SS_GoStageRoot.EKind.PacVillage_Sub)
            {
                return;
            }
            __result = __result && ServiceFactory.GameSaveDataService.GetStageFlag((EWorldStage)stageObj.StageInfo.stageId) != EStageFlag.Locked;
        }
    }
}
