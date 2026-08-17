using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using MelonLoader;
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
    public class LocationsService : ILocationsService, IAPClientEventHandler
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

            _connectionService.HandleEvents(this);
        }

        public IImmutableSet<EWorldStage> ClearedStages => _clearedStages.ToImmutableHashSet();

        public void ClearStage(EWorldStage stage)
        {
            _clearedStages.Add(stage);
        }

        public void InitItems(IReadOnlyList<ItemInfo> items)
        {

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

        public void ItemReceived(ItemInfo item)
        {

        }

        public void LocationCheckedRemotely(long locationId)
        {

        }

        public void OnConnect()
        {

        }

        public void OnLateUpdate()
        {
            _sendStagesCleared();
        }

        private void clearSentLocations()
        {

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
                _sentStageClears.Add(stage);
            }
        }
    }
}
