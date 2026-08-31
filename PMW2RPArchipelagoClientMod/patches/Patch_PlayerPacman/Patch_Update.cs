using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    [HarmonyPatch(typeof(PlayerPacman), "Update", [])]
    public class Patch_Update
    {
        private static void Prefix(PlayerPacman __instance)
        {
            ServiceFactory.PlayerPacmanStateService.SetSuperDK(__instance);
        }
    }
}
