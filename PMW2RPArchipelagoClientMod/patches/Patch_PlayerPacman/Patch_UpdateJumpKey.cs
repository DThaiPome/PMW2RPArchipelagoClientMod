using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    /// <summary>
    /// Butt bouncing calls "EndJump" first then "SetHipAtk". We need to cancel "EndJump" after a key press to prevent the player from infinitely gliding.
    /// </summary>
    [HarmonyPatch(typeof(PlayerPacman), "UpdateJumpKey", [])]
    public class Patch_UpdateJumpKey
    {
        private static void Prefix(PlayerPacman __instance)
        {
            if (GameUtil.UpdateKeyState(__instance.m_padNum, __instance.m_playerNo, EKeyAsign.Jump, __instance.m_jumpKeyState) == EKeyState.KeyDown
                && ServiceFactory.GetUnlocks().ButtBounce == ProgressiveButtBounce.None)
            {
                ServiceFactory.GetPlayerPacmanStateService().PushSkipEndJump();
            }
        }
    }
}
