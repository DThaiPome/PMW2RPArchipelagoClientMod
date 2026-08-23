using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.game;
using PMW2RPArchipelagoClientMod.services.items.mapping;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private void _sendStagesCleared()
        {
            foreach (EWorldStage stage in _clearedStages)
            {
                long locationId = _checkIdMapperService.StageToClearStageLocationId(stage);
                if (_sentLocations.Contains(locationId))
                {
                    continue;
                }
                _connectionService.SendLocationChecked(locationId);
                if ((stage == EWorldStage.Stage6_4 && _connectionService.GoalBoss == GoalBossOption.Spooky)
                    || (stage == EWorldStage.Stage6_5 && _connectionService.GoalBoss == GoalBossOption.TocMan))
                {
                    _connectionService.Goal();
                }
                _sentLocations.Add(locationId);
            }
        }

        private void _sendMissionsCleared()
        {
            foreach (EMissionKind kind in _clearedMissions)
            {
                long locationId = _checkIdMapperService.MissionToClearMissionLocationId(kind);
                if (_sentLocations.Contains(locationId))
                {
                    continue;
                }
                _connectionService.SendLocationChecked(locationId);
                _sentLocations.Add(locationId);
            }
        }
    }
}
