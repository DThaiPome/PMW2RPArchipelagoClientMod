using HarmonyLib;
using Il2Cpp;
using Il2CppPacman;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    [HarmonyPatch(typeof(PlayerPacman), "IsSuperHipAtkOKJump")]
    public class Patch_IsSuperHipAtkOKJump
    {
        private static bool Prefix(ref bool __result, EJumpKind jump)
        {
            if (ServiceFactory.Unlocks.ButtBounce != ProgressiveButtBounce.SuperButtBounce)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
