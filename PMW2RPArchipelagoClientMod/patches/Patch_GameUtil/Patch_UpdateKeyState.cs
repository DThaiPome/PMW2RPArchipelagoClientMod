using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items;

namespace PMW2RPArchipelagoClientMod.patches.Patch_GameUtil
{
    [HarmonyPatch(typeof(GameUtil), "UpdateKeyState", [typeof(int), typeof(EPlayerNo), typeof(EKeyAsign), typeof(EKeyState)])]
    public class Patch_UpdateKeyState
    {
        private static void Postfix(int padNum, EPlayerNo no, EKeyAsign asign, EKeyState currentState, ref EKeyState __result)
        {
            var unlocks = ServiceFactory.Unlocks;
            __result = asign switch
            {
                EKeyAsign.FlipKick => _overrideKeyState(__result, MovesetItem.IsFlipKickReceived(unlocks)),
                EKeyAsign.PacDash => _overrideKeyState(__result, MovesetItem.IsRevRollReceived(unlocks)),
                EKeyAsign.DotAttack => _overrideKeyState(__result, MovesetItem.IsDotThrowReceived(unlocks)),
                EKeyAsign.Hunbari => _overrideKeyState(__result, MovesetItem.IsFlutterReceived(unlocks)),
                EKeyAsign.DolphinKick => _overrideKeyState(__result, MovesetItem.GetDolphinKickLevel(unlocks) != ProgressiveDolphinKick.None),
                _ => __result
            };
        }

        private static EKeyState _overrideKeyState(EKeyState result, bool unlocked)
        {
            return unlocked ? result : EKeyState.Removed;
        }
    }
}
