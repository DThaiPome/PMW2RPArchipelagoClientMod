using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items.mapping;
using System.Collections.Immutable;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public class LocationsService : ILocationsService
    {
        private MelonMod _melonMod;
        private IAPConnectionService _connectionService;
        private ICheckIdMapperService _checkIdMapperService;

        private ISet<long> _sentLocations = new HashSet<long>();
        private ISet<EWorldStage> _clearedStages = new HashSet<EWorldStage>();
        private ISet<EMissionKind> _clearedMissions = new HashSet<EMissionKind>();
        private ISet<int> _unlockedMazes = new HashSet<int>();
        private ISet<ECapsule> _collectedCapsules = new HashSet<ECapsule>();
        private ISet<EWorldStage> _goldMedals = new HashSet<EWorldStage>();


        public LocationsService(MelonMod melonMod,
            IAPConnectionService connectionService,
            ICheckIdMapperService checkIdMapperService)
        {
            _melonMod = melonMod;
            _connectionService = connectionService;
            _checkIdMapperService = checkIdMapperService;

            _connectionService.InitLocations += InitLocations;
            _connectionService.LocationCheckedRemotely += LocationCheckedRemotely;
        }

        public IImmutableSet<EWorldStage> ClearedStages => _clearedStages.ToImmutableHashSet();

        public IImmutableSet<EMissionKind> ClearedMissions => _clearedMissions.ToImmutableHashSet();

        public IImmutableSet<int> UnlockedMazes => _unlockedMazes.ToImmutableHashSet();

        public IImmutableSet<ECapsule> CollectedCapsules => _collectedCapsules.ToImmutableHashSet();

        public IImmutableSet<EWorldStage> ClearedGoldMedals => _goldMedals.ToImmutableHashSet();

        public void ClearMission(EMissionKind kind)
        {
            _clearedMissions.Add(kind);
        }

        public void ClearStage(EWorldStage stage)
        {
            _clearedStages.Add(stage);
        }

        public void InitLocations(IReadOnlyList<long> locationIds)
        {
            _sentLocations.Clear();
            _clearedStages.Clear();
            _clearedMissions.Clear();
            _unlockedMazes.Clear();
            _collectedCapsules.Clear();
            foreach (var locationId in locationIds)
            {
                _checkIdMapperService.MapLocation(locationId).ClearLocation(this);
            }
        }

        public void LocationCheckedRemotely(long locationId)
        {
            _checkIdMapperService.MapLocation(locationId).ClearLocation(this);
        }

        public void OnLateUpdate()
        {
            _sendStagesCleared();
            _sendMissionsCleared();
            _sendMazesUnlocked();
            _sendCapsulesCollected();
            _sendGoldMedalCleared();
        }

        private void _sendStagesCleared()
        {
            foreach (EWorldStage stage in _clearedStages)
            {
                if (!_sendLocationClearedIfNeeded(_checkIdMapperService.StageToClearStageLocationId(stage)))
                {
                    continue;
                }
                if ((stage == EWorldStage.Stage6_4 && _connectionService.GoalBoss == GoalBossOption.Spooky)
                    || (stage == EWorldStage.Stage6_5 && _connectionService.GoalBoss == GoalBossOption.TocMan))
                {
                    _connectionService.Goal();
                }
            }
        }

        private void _sendMissionsCleared()
        {
            foreach (EMissionKind kind in _clearedMissions)
            {
                _sendLocationClearedIfNeeded(_checkIdMapperService.MissionToClearMissionLocationId(kind));
            }
        }

        private void _sendMazesUnlocked()
        {
            foreach (int mazeId in _unlockedMazes)
            {
                _sendLocationClearedIfNeeded(_checkIdMapperService.MazeUnlockToUnlockMazeLocationId(mazeId));
            }
        }

        private void _sendCapsulesCollected()
        {
            foreach (ECapsule capsule in _collectedCapsules)
            {
                _sendLocationClearedIfNeeded(_checkIdMapperService.CapsuleToCollectCapsuleLocationId(capsule));
            }
        }

        private void _sendGoldMedalCleared()
        {
            foreach (EWorldStage stage in _goldMedals)
            {
                _sendLocationClearedIfNeeded(_checkIdMapperService.StageToClearedGoldMedalLocationId(stage));
            }
        }

        private bool _sendLocationClearedIfNeeded(long locationId)
        {
            if (_sentLocations.Contains(locationId))
            {
                return false;
            }
            _connectionService.SendLocationChecked(locationId);
            _sentLocations.Add(locationId);
            return true;
        }

        public void UnlockMaze(int mazeId)
        {
            _unlockedMazes.Add(mazeId);
        }

        public void CollectCapsule(ECapsule capsule)
        {
            _collectedCapsules.Add(capsule);
        }

        public void ClearGoldMedal(EWorldStage stage)
        {
            _goldMedals.Add(stage);
        }
    }
}
