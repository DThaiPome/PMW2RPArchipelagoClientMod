using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_SS_GoStageRoot
{
    [HarmonyPatch(typeof(SS_GoStageRoot), "get_CanGoStage", [])]
    public class SS_GoStageRoot_get_CanGoStage
    {
        private static void Postfix(SS_GoStageRoot __instance, ref bool __result)
        {
            __result = __result &&
                ((__instance.Kind == SS_GoStageRoot.EKind.PacVillage || __instance.Kind == SS_GoStageRoot.EKind.PacVillage_Sub) 
                || ServiceFactory.GetUnlocks().Stages.GetValueOrDefault((EWorldStage)__instance.StageInfo.stageId, false));
        }
    }
}
