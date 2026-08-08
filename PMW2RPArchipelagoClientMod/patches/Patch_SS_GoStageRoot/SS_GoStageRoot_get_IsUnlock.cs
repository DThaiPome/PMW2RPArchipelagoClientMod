using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2Cpp;

namespace PMW2RPArchipelagoClientMod.patches.Patch_SS_GoStageRoot
{
    [HarmonyPatch(typeof(SS_GoStageRoot), "get_IsUnlock", [])]
    public class SS_GoStageRoot_get_IsUnlock
    {
        private static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
