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

        private ISet<EWorldStage> _clearedStages = new HashSet<EWorldStage>();
        private ISet<EWorldStage> _sentStageClears = new HashSet<EWorldStage>();

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

        public void ClearStage(EWorldStage stage)
        {
            _clearedStages.Add(stage);
        }

        public void InitLocations(IReadOnlyList<long> locationIds)
        {
            _clearedStages.Clear();
            _sentStageClears.Clear();
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
        }

        private void _sendStagesCleared()
        {
            foreach (EWorldStage stage in _clearedStages)
            {
                if (_sentStageClears.Contains(stage))
                {
                    continue;
                }
                _connectionService.SendLocationChecked(_checkIdMapperService.StageToClearStageLocationId(stage));
                if ((stage == EWorldStage.Stage6_4 && _connectionService.GoalBoss == GoalBossOption.Spooky)
                    || (stage == EWorldStage.Stage6_5 && _connectionService.GoalBoss == GoalBossOption.TocMan))
                {
                    _connectionService.Goal();
                }
                _sentStageClears.Add(stage);
            }
        }
    }
}
