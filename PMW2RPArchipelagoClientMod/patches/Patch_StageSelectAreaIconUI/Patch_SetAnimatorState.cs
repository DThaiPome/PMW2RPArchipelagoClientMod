using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.services.items.item.items;

namespace PMW2RPArchipelagoClientMod.patches.Patch_StageSelectAreaIconUI
{
    [HarmonyPatch(typeof(StageSelectAreaIconUI), "SetAnimatorState")]
    public class Patch_SetAnimatorState
    {
        private static void Prefix(ref StageSelectAreaIconUI.EAnimatorState state, StageSelectAreaIconUI __instance)
        {
            bool isLevelRando = ServiceFactory.APConnectionService.IsLevelRando ?? true;

            EWorldStage stage = (EWorldStage?)__instance.StageRoot?.StageInfo?.stageId ?? EWorldStage.PacVillage;
            if (stage == EWorldStage.PacVillage)
            {
                return;
            }

            bool stageUnlocked = isLevelRando ? StageItem.IsStageReceived(ServiceFactory.Unlocks, stage) : ServiceFactory.GameSaveDataService.GetStageFlag(stage) != EStageFlag.Locked;
            state = stageUnlocked ? state : StageSelectAreaIconUI.EAnimatorState.DISABLED;
            return;
        }
    }
}
