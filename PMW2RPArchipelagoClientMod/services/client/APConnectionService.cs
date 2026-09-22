using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.util;
using System.Collections.ObjectModel;

namespace PMW2RPArchipelagoClientMod.services.client
{
    public class APConnectionService : IAPConnectionService
    {
        private MelonMod _melonMod;
        private IAPSessionService _apSessionService;

        private Dictionary<string, object> _slotData;

        private List<ItemInfo> _initItems = new List<ItemInfo>();

        private object _xItemsInitializedLock = new();
        private bool _xItemsInitialized;
        private bool _xCanInitItems;

        public Action OnConnect { get; set; }
        public Action<IReadOnlyList<ItemInfo>> InitItems { get; set; }
        public Action<IReadOnlyList<long>> InitLocations { get; set; }
        public Action<ItemInfo> ItemReceived { get; set; }
        public Action<long> LocationCheckedRemotely { get; set; }

        public GoalBossOption? GoalBoss => _goalBossOption;
        public bool? IsLevelRando => _isLevelRando;

        public APConnectionService(MelonMod melonMod, IAPSessionService apSessionService)
        {
            _melonMod = melonMod;
            _apSessionService = apSessionService;

            _apSessionService.OnAboutToConnect += _resetConnectStates;
            _apSessionService.OnConnectionSuccessful += _onLoginSuccess;
            _apSessionService.OnItemReceived += _onItemReceived;

            lock(_xItemsInitializedLock)
            {
                _xItemsInitialized = true;
                _xCanInitItems = false;
            }
        }

        private void _resetConnectStates()
        {
            _initItems.Clear();
            lock(_xItemsInitializedLock)
            {
                _xItemsInitialized = false;
                _xCanInitItems = false;
            }
        }

        private void _onLoginSuccess(IAPSessionService.IConnectionSuccessResponse response)
        {
            _slotData = new Dictionary<string, object>(response.SlotData);
            OnConnect?.Invoke();
            InitLocations?.Invoke(response.LocationsCleared);
            lock(_xItemsInitializedLock)
            {
                _xCanInitItems = true;
            }
        }

        private void _onItemReceived(ItemInfo item)
        {
            lock(_xItemsInitializedLock)
            {
                if (!_xItemsInitialized)
                {
                    _initItems.Add(item);
                }
                else
                {
                    ItemReceived?.Invoke(item);
                }
            }
        }

        private void _onCheckedLocationsUpdated(ReadOnlyCollection<long> newCheckedLocations)
        {
            foreach (long id in newCheckedLocations)
            {
                LocationCheckedRemotely?.Invoke(id);
            }
        }

        public void SendLocationChecked(long id)
        {
            _sendLocationsChecked([id]);
        }

        public void SendLocationsChecked(long[] ids)
        {
            _sendLocationsChecked(ids);
        }

        private void _sendLocationsChecked(long[] ids)
        {
            _apSessionService.ClearLocations(ids);
        }

        public void OnLateUpdate()
        {
            _initItemsIfNeeded();
        }

        private void _initItemsIfNeeded()
        {
            lock(_xItemsInitializedLock)
            {
                if (!_xCanInitItems || _xItemsInitialized)
                {
                    return;
                }
                InitItems?.Invoke(_initItems);
                _xItemsInitialized = true;
                _xCanInitItems = false;
                _initItems.Clear();
            }
        }

        public void Goal()
        {
            _apSessionService.SetGoalAchieved();
        }

        private GoalBossOption? _goalBossOption
        {
            get
            {
                if (_slotData == null)
                {
                    return null;
                }
                long? goalBossId = (long?)_slotData.GetValueOrDefault("goal_boss", null);
                if (goalBossId == null)
                {
                    return null;
                }
                return (GoalBossOption)goalBossId;
            }
        }

        private bool? _isLevelRando
        {
            get
            {
                if (_slotData == null)
                {
                    return null;
                }
                long? isLevelRando = (long?)_slotData.GetValueOrDefault("level_randomizer", null);
                if (isLevelRando == null)
                {
                    return null;
                }
                return isLevelRando == 1;
            }
        } 
    }
}
