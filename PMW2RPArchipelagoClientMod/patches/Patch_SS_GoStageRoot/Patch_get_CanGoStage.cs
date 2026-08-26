using Il2Cpp;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_SS_GoStageRoot
{
    [HarmonyPatch(typeof(SS_GoStageRoot), "get_CanGoStage", [])]
    public class Patch_get_CanGoStage
    {
        private static void Postfix(SS_GoStageRoot __instance, ref bool __result)
        {
            __result = __result &&
                ((__instance.Kind == SS_GoStageRoot.EKind.PacVillage || __instance.Kind == SS_GoStageRoot.EKind.PacVillage_Sub) 
                || ServiceFactory.GameSaveDataService.GetStageFlag((EWorldStage)__instance.StageInfo.stageId) != EStageFlag.Locked);
        }
    }
}
