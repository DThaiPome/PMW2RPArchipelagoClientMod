using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_StageSelectAreaIconUI
{
    [HarmonyPatch(typeof(StageSelectAreaIconUI), "SetAnimatorState")]
    public class Patch_SetAnimatorState
    {
        private static void Prefix(ref StageSelectAreaIconUI.EAnimatorState state, StageSelectAreaIconUI __instance)
        {
            if (!(ServiceFactory.APConnectionService.IsLevelRando ?? true))
            {
                return;
            }

            EWorldStage stage = (EWorldStage?)__instance.StageRoot?.StageInfo?.stageId ?? EWorldStage.PacVillage;
            if (stage == EWorldStage.PacVillage)
            {
                return;
            }

            state = ServiceFactory.Unlocks.Stages.GetValueOrDefault(stage, false) ? state : StageSelectAreaIconUI.EAnimatorState.DISABLED;
            return;
        }
    }
}
