using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services;

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
                EKeyAsign.FlipKick => _overrideKeyState(__result, unlocks.FlipKick),
                EKeyAsign.PacDash => _overrideKeyState(__result, unlocks.Dash),
                EKeyAsign.DotAttack => _overrideKeyState(__result, unlocks.Bomb),
                EKeyAsign.Hunbari => _overrideKeyState(__result, unlocks.Flutter),
                EKeyAsign.DolphinKick => _overrideKeyState(__result, unlocks.DolphinKick != ProgressiveDolphinKick.None),
                _ => __result
            };
        }

        private static EKeyState _overrideKeyState(EKeyState result, bool unlocked)
        {
            return unlocked ? result : EKeyState.Removed;
        }
    }
}
