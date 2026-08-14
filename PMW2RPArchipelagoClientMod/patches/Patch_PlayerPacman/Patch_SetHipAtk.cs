using Il2Cpp;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;
using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    [HarmonyPatch(typeof(PlayerPacman), "SetHipAtk")]
    public class Patch_SetHipAtk
    {
        private static bool Prefix(bool flag, ref bool super)
        {
            ProgressiveButtBounce buttBounce = ServiceFactory.Unlocks.ButtBounce;
            if (buttBounce == ProgressiveButtBounce.SuperButtBounce)
            {
                return true;
            }
            if (buttBounce == ProgressiveButtBounce.ButtBounce)
            {
                super = false;
                return true;
            }
            return false;
        }
    }
}
