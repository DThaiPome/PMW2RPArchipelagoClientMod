using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_GimmickGoStage
{
    [HarmonyPatch(typeof(GimmickGoStage), "get_IsExecOK", [])]
    public class GimmickGoStage_IsExecOK
    {
        private static void Postfix(GimmickGoStage __instance, ref bool __result)
        {
            SS_GoStageRoot stageObj = __instance.m_rootScript;
            if (stageObj == null || stageObj.Kind == SS_GoStageRoot.EKind.PacVillage || stageObj.Kind == SS_GoStageRoot.EKind.PacVillage_Sub)
            {
                return;
            }
            __result = __result && ServiceFactory.GetUnlocks().Stages.GetValueOrDefault((EWorldStage)stageObj.StageInfo.stageId, false);
        }
    }
}
