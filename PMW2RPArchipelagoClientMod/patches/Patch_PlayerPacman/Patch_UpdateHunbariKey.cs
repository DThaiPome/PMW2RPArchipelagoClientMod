using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PlayerPacman
{
    [HarmonyPatch(typeof(PlayerPacman), "UpdateHunbariKey", [])]
    public class Patch_UpdateHunbariKey
    {
        private static bool Prefix(PlayerPacman __instance)
        {
            if (ServiceFactory.Unlocks.Flutter)
            {
                return true;
            }
            __instance.m_hunbaKeyState = GameUtil.UpdateKeyState(__instance.m_padNum, __instance.m_playerNo, EKeyAsign.Hunbari, __instance.m_dashKeyState);
            return false;
        }
    }
}
