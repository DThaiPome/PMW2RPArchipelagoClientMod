using Il2Cpp;
using System.Collections.Immutable;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public interface ILocationsSource
    {
        IImmutableSet<EWorldStage> ClearedStages { get; }
        void ClearStage(EWorldStage stage);
        IImmutableSet<EMissionKind> ClearedMissions { get; }
        void ClearMission(EMissionKind kind);
        IImmutableSet<int> UnlockedMazes { get; }
        void UnlockMaze(int mazeId);
        IImmutableSet<ECapsule> CollectedCapsules { get; }
        void CollectCapsule(ECapsule capsule);
        IImmutableSet<EWorldStage> ClearedGoldMedals { get; }
        void ClearGoldMedal(EWorldStage stage);
    }
}
