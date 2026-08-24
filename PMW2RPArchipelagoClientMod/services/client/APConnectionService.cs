using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.client
{
    public class APConnectionService : IAPConnectionService
    {
        private MelonMod _melonMod;

        private ArchipelagoSession _session;
        private Dictionary<string, object> _slotData;

        private static readonly long ITEMS_INIT_THRESHOLD_MS = 250;

        private long _lastConnectMs;
        private List<ItemInfo> _initItems = new List<ItemInfo>();

        private object _xItemsInitializedLock = new();
        private bool _xItemsInitialized;

        public Action OnConnect { get; set; }
        public Action<IReadOnlyList<ItemInfo>> InitItems { get; set; }
        public Action<IReadOnlyList<long>> InitLocations { get; set; }
        public Action<ItemInfo> ItemReceived { get; set; }
        public Action<long> LocationCheckedRemotely { get; set; }

        public GoalBossOption? GoalBoss => _goalBossOption;

        public APConnectionService(MelonMod melonMod)
        {
            _melonMod = melonMod;
            _session = null;

            lock(_xItemsInitializedLock)
            {
                _xItemsInitialized = true;
            }
        }

        public bool CloseSession()
        {
            if (_session == null)
            {
                return false;
            }
            _session.Socket.DisconnectAsync();
            _session = null;
            return true;
        }

        public bool CreateSessionAndLogIn(string domain, int port, string slotName, string password = null)
        {
            try
            {
                var session = ArchipelagoSessionFactory.CreateSession(domain, port);
                _resetConnectStates();
                session.Items.ItemReceived += _onItemReceived;
                session.Locations.CheckedLocationsUpdated += _onCheckedLocationsUpdated;
                var loginResult = session.TryConnectAndLogin("Pac-Man World 2 Re-Pac", slotName, ItemsHandlingFlags.AllItems, version: new Version("0.6.7"), password: password);
                if (loginResult.Successful)
                {
                    _lastConnectMs = DateTime.Now.Millisecond;
                    _session = session;
                    _onLoginSuccess();
                }
                return loginResult.Successful;
            }
            catch (Exception ex)
            {
                _melonMod.LoggerInstance.Error(ex);
                return false;
            }
        }

        private void _resetConnectStates()
        {
            _initItems.Clear();
            lock(_xItemsInitializedLock)
            {
                _xItemsInitialized = false;
            }
        }

        private void _onLoginSuccess()
        {
            _melonMod.LoggerInstance.Msg("CONNECTED TO SERVER");
            OnConnect?.Invoke();
            InitLocations?.Invoke(_session.Locations.AllLocationsChecked);
            _slotData = new Dictionary<string, object>(_session.DataStorage.GetSlotData());
        }

        private void _onItemReceived(ReceivedItemsHelper helper)
        {
            lock(_xItemsInitializedLock)
            {
                ItemInfo? item = helper.DequeueItem();
                while (item != null)
                {
                    if (!_xItemsInitialized)
                    {
                        _initItems.Add(item);
                    }
                    else
                    {
                        ItemReceived?.Invoke(item);
                    }
                    item = helper.DequeueItem();
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
            if (_session == null)
            {
                return;
            }
            _session.Locations.CompleteLocationChecks(ids);
        }

        public void OnLateUpdate()
        {
            _initItemsIfNeeded();
        }

        private void _initItemsIfNeeded()
        {
            lock(_xItemsInitializedLock)
            {
                if ((DateTime.Now.Millisecond - _lastConnectMs < ITEMS_INIT_THRESHOLD_MS) || _xItemsInitialized)
                {
                    return;
                }
                _xItemsInitialized = true;
                InitItems?.Invoke(_initItems);
                _initItems.Clear();
            }
        }

        public void Goal()
        {
            if (_session == null)
            {
                return;
            }
            _session.SetGoalAchieved();
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
    }
}
