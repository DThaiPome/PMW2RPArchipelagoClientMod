using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items.location.locations;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.location
{
    public class LocationsService : ILocationsService, ILocationsDispatcher
    {
        private MelonMod _melonMod;
        private IAPConnectionService _apConnectionService;
        private IIdMapperService _idMapperService;

        private HashSet<IUnlockableLocationId> _clearedLocations = new HashSet<IUnlockableLocationId>();
        private HashSet<long> _sentLocations = new HashSet<long>();

        public LocationsService(MelonMod melonMod,
            IAPConnectionService apConnectionService,
            IIdMapperService idMapperService)
        {
            _melonMod = melonMod;
            _apConnectionService = apConnectionService;
            _idMapperService = idMapperService;

            _apConnectionService.InitLocations += _onInitLocations;
            _apConnectionService.LocationCheckedRemotely += _onLocationCheckedRemotely;
        }

        private void _onInitLocations(IReadOnlyList<long> locationIds)
        {
            _resetLocations();
            foreach (long id in locationIds)
            {
                _receiveLocation(id);
            }
        }

        private void _onLocationCheckedRemotely(long locationId)
        {
            _receiveLocation(locationId);
        }

        private void _receiveLocation(long locationId)
        {
            if (_idMapperService.TryGetLocationFromId(locationId, out var location))
            {
                location.Clear(this);
            }
        }

        private void _resetLocations()
        {
            _clearedLocations.Clear();
            _sentLocations.Clear();
        }

        public void Clear(IUnlockableLocationId location)
        {
            _clearLocation(location);
        }

        public void ClearLocation(IUnlockableLocationId location)
        {
            _clearLocation(location);
        }

        private void _clearLocation(IUnlockableLocationId location)
        {
            _clearedLocations.Add(location);
        }

        public bool IsCleared(IUnlockableLocationId location)
        {
            return _clearedLocations.Contains(location);
        }

        public void OnLateUpdate()
        {
            _sendLocationsCleared();
        }

        private void _sendLocationsCleared()
        {
            foreach (var location in _clearedLocations)
            {
                if (!_sendLocationClearedIfNeeded(location))
                {
                    continue;
                }
                if (location.Equals(StageClearLocation.STAGE_CLEAR_LOCATIONS[EWorldStage.Stage6_4]) && _apConnectionService.GoalBoss == GoalBossOption.Spooky
                    || location.Equals(StageClearLocation.STAGE_CLEAR_LOCATIONS[EWorldStage.Stage6_5]) && _apConnectionService.GoalBoss == GoalBossOption.TocMan)
                {
                    _apConnectionService.Goal();
                }
            }
        }

        private bool _sendLocationClearedIfNeeded(IUnlockableLocationId location)
        {
            long id = location.Id;
            if (_sentLocations.Contains(id))
            {
                return false;
            }
            _apConnectionService.SendLocationChecked(id);
            _sentLocations.Add(id);
            return true;
        }
    }
}
