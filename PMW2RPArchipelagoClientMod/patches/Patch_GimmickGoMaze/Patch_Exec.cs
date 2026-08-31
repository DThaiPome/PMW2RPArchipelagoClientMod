using Il2Cpp;
using HarmonyLib;
using System.Numerics;
using PMW2RPArchipelagoClientMod.services;

namespace PMW2RPArchipelagoClientMod.patches.Patch_GimmickGoMaze
{
    [HarmonyPatch(typeof(GimmickGoMaze), "Exec")]
    public class Patch_Exec
    {
        private static void Prefix(GimmickGoMaze __instance, PlayerPacman pacman, EGimmickTrigger trigger, Vector3 pos)
        {
            MazeInfo info = MasterData.GetMaze(__instance.m_nextScene);
            ServiceFactory.GameSaveDataService.UnlockMaze(info.mazeId);
        }
    }
}
