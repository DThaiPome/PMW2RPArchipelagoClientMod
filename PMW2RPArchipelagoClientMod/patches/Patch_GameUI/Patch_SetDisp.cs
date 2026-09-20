using Il2CppUI;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_GameUI
{
    [HarmonyPatch(typeof(GameUI), "SetDisp")]
    public class Patch_SetDisp
    {
        private static void Prefix(bool sw, bool withADV = true)
        {
            ServiceFactory.PlayerPacmanStateService.UpdateUIVisible(sw);
        }
    }
}
