using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    [HarmonyPatch(typeof(PlayerPacman), "UpdateDashKey", [])]
    public class Patch_UpdateDashKey
    {
        private static bool Prefix(PlayerPacman __instance)
        {
            if (ServiceFactory.Unlocks.Dash)
            {
                return true;
            }
            __instance.m_dashKeyState = GameUtil.UpdateKeyState(__instance.m_padNum, __instance.m_playerNo, EKeyAsign.PacDash, __instance.m_dashKeyState);
            return false;
        }
    }
}
