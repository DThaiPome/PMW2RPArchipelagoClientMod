using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    [HarmonyPatch(typeof(PlayerPacman), "CheckFlipKickKey", [])]
    public class Patch_CheckFlipKickKey
    {
        private static bool Prefix()
        {
            if (ServiceFactory.Unlocks.FlipKick)
            {
                return true;
            }
            return false;
        }
    }
}
