using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_SS_GoStageRoot
{
    [HarmonyPatch(typeof(SS_GoStageRoot), "get_IsUnlock", [])]
    public class Patch_get_IsUnlock
    {
        private static bool Prefix(ref bool __result)
        {
            if (!(ServiceFactory.APConnectionService.IsLevelRando ?? true))
            {
                return true;
            }
            __result = true;
            return false;
        }
    }
}
