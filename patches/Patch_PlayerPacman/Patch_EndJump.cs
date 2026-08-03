using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    /// <summary>
    /// Butt bouncing calls "EndJump" first then "SetHipAtk". We need to cancel "EndJump" after a key press to prevent the player from infinitely gliding.
    /// </summary>
    [HarmonyPatch(typeof(PlayerPacman), "EndJump")]
    public class Patch_EndJump
    {
        private static bool Prefix()
        {
            return !ServiceFactory.GetPlayerPacmanStateService().PopSkipEndJump();
        }
    }
}
