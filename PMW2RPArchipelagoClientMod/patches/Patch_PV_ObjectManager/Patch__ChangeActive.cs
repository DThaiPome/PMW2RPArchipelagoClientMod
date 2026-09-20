using HarmonyLib;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_PV_ObjectManager
{
    [HarmonyPatch(typeof(PV_ObjectManager), "_ChangeActive")]
    public class Patch__ChangeActive
    {
        private static void PostFix(EPacVillageCond cond)
        {
            ServiceFactory.GoalCheckVisibilityService.OnSceneWasInitialized();
        }
    }
}
