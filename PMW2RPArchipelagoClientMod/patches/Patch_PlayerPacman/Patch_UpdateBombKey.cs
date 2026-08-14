using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    [HarmonyPatch(typeof(PlayerPacman), "UpdateBombKey", [])]
    public class Patch_UpdateBombKey
    {
        private static bool Prefix(PlayerPacman __instance)
        {
            if (ServiceFactory.Unlocks.Bomb)
            {
                return true;
            }
            __instance.m_bombKeyState = GameUtil.UpdateKeyState(__instance.m_padNum, __instance.m_playerNo, EKeyAsign.DotAttack, __instance.m_bombKeyState);
            return false;
        }
    }
}
