using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
