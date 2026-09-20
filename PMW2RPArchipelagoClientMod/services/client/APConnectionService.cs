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
        private static readonly string CONNECTION_INI_PATH = "./UserData/ap_connection.ini";

        private MelonMod _melonMod;

        private ArchipelagoSession _session;
        private Dictionary<string, object> _slotData;
        private bool _connectionNotConfigured = false;

        private static readonly long ITEMS_INIT_THRESHOLD_MS = 500;

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
        public bool? IsLevelRando => _isLevelRando;

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
                    _lastConnectMs = TimeUtil.NowMs();
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
            _melonMod.LoggerInstance.Msg("SUCCESS - CONNECTED TO SERVER: " + _session.Socket.Uri);
            _slotData = new Dictionary<string, object>(_session.DataStorage.GetSlotData());
            OnConnect?.Invoke();
            InitLocations?.Invoke(_session.Locations.AllLocationsChecked);
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
            _initConnectionIfNeeded();
            _initItemsIfNeeded();
        }

        private void _initConnectionIfNeeded()
        {
            if (_session != null || _connectionNotConfigured)
            {
                return;
            }

            if (!File.Exists(CONNECTION_INI_PATH))
            {
                _melonMod.LoggerInstance.Warning(string.Format("CREATING CONNECTION CONFIG FILE: Configure your connection at {0}, then restart the game", CONNECTION_INI_PATH));
                _writeIniFileTemplate();
                _connectionNotConfigured = true;
                return;
            }

            string[] lines = File.ReadAllLines(CONNECTION_INI_PATH);
            string domain = null;
            int? port = null;
            string slot = null;
            string password = null;

            foreach (string line in lines)
            {
                if (line.StartsWith("domain="))
                {
                    domain = line.Split('=')[1];
                    continue;
                }
                if (line.StartsWith("port="))
                {
                    int _port;
                    if (!int.TryParse(line.Split('=')[1], out _port))
                    {
                        _melonMod.LoggerInstance.Error("BAD CONNECTION CONFIG: Port must be a number");
                        _connectionNotConfigured = true;
                        return;
                    }
                    port = _port;
                    continue;
                }
                if (line.StartsWith("slot="))
                {
                    slot = line.Split('=')[1];
                    continue;
                }
                if (line.StartsWith("password="))
                {
                    password = line.Split('=')[1];
                    if (string.IsNullOrEmpty(password))
                    {
                        password = null;
                    }
                    continue;
                }
            }

            if (domain == null || port == null || slot == null)
            {
                _melonMod.LoggerInstance.Error("BAD CONNECTION CONFIG: MISSING FIELDS");
                _connectionNotConfigured = true;
                return;
            }

            CreateSessionAndLogIn(domain, (int)port, slot, password);
        }

        private void _writeIniFileTemplate()
        {
            File.WriteAllLines(CONNECTION_INI_PATH, new string[]{
                "# The multiorld server domain",
                "domain=archipelago.gg",
                "# The multiworld port",
                "port=38281",
                "# The name of your player slot in the multiworld",
                "slot=DThaiPome_PMW2RP",
                "# The multiworld password (leave this blank if there is no password)",
                "password="
            });
        }

        private void _initItemsIfNeeded()
        {
            lock(_xItemsInitializedLock)
            {
                if ((TimeUtil.NowMs() - _lastConnectMs < ITEMS_INIT_THRESHOLD_MS) || _xItemsInitialized)
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
