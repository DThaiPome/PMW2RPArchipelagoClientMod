using Archipelago.MultiClient.Net.Models;
using Il2Cpp;

namespace PMW2RPArchipelagoClientMod.services.items.mapping
{
    public interface ICheckIdMapperService
    {
        IItemMapEntry MapItem(ItemInfo itemInfo);
        long StageToClearStageLocationId(EWorldStage stage);
        long MissionToClearMissionLocationId(EMissionKind kind);
        long MazeUnlockToUnlockMazeLocationId(int mazeId);
        long CapsuleToCollectCapsuleLocationId(ECapsule capsule);
        long StageToClearedGoldMedalLocationId(EWorldStage stage);
        ILocationMapEntry MapLocation(long id);
    }
}
