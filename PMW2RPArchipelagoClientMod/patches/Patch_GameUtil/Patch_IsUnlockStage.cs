using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_GameUtil
{
    //TODO: It would be cool if for every level you unlock in the rando, it plays the level unlock cutscene in the map.
    // Skiping this for now because I just want to get this working.
    [HarmonyPatch(typeof(GameUtil), "IsUnlockStage")]
    public class Patch_IsUnlockStage
    {
        private static void Postfix(Il2CppSystem.Collections.Generic.List<int> unlockStageIdList, Il2CppSystem.Collections.Generic.List<int> defaultStageIdList, ref bool __result)
        {
            foreach (var stage in ServiceFactory.StageSelectCinematicService.FlushUnlocks())
            {
                unlockStageIdList.Add((int)stage);
            }
            __result = __result || unlockStageIdList.Count > 0;
        }
    }
}
