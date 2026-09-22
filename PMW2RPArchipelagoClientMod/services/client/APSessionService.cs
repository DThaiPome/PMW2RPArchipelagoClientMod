using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using Il2CppCoffee.UIExtensions;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.client
{
    public class APSessionService : IAPSessionService
    {
        private MelonMod _melonMod;
        private bool _connectionConfigured;

        private static readonly string CONNECTION_INI_PATH = "./UserData/ap_connection.ini";
        private static readonly string GAME_NAME = "Pac-Man World 2 Re-Pac";
        private static readonly string AP_VERSION = "0.6.7";

        private string _slotName;
        private string _password;

        private ArchipelagoSession _session;

        public Action OnAboutToConnect { get; set; }
        public Action<IAPSessionService.IConnectionSuccessResponse> OnConnectionSuccessful { get; set; }
        public Action<ItemInfo> OnItemReceived { get; set; }
        public Action<long> OnLocationClearedRemotely { get; set; }

        public APSessionService(MelonMod melonMod)
        {
            _melonMod = melonMod;
            _connectionConfigured = _initConnectionInfoFromFile();
            if (!_connectionConfigured)
            {
                _melonMod.LoggerInstance.Error(string.Format("WILL NOT CONNECT TO MULTIWORLD UNTIL CONNECTION IS CONFIGURED", CONNECTION_INI_PATH));
            }
        }

        public void OnLateUpdate()
        {
            _updateSession();
        }
        private void _updateSession()
        {
            if (!_connectionConfigured)
            {
                return;
            }

            if (_connecting())
            {
                return;
            }

            if (_loggingIn())
            {
                return;
            }
        }

        private Task<RoomInfoPacket> _sessionConnectTask = null;
        private bool _firstConnectionAtempt = true;
        private bool _connectedSuccessfully = false;

        private bool _connecting()
        {
            if (_session.Socket.Connected && _sessionConnectTask != null && _sessionConnectTask.IsCompleted && !_sessionConnectTask.IsCanceled)
            {
                if (!_connectedSuccessfully)
                {
                    _melonMod.LoggerInstance.Msg(ConsoleColor.Green, "CONNECTED TO " + _prettyPrintSessionUri + " SUCCESSFULLY");
                }
                _connectedSuccessfully = true;
                return false;
            }
            if (_firstConnectionAtempt || _connectedSuccessfully)
            {
                _firstConnectionAtempt = false;
                _connectedSuccessfully = false;
                _broadcastAboutToConnect();
            }
            if (_isConnectTaskStillWorking())
            {
                return true;
            }
            _resetLoginState();
            _startConnect();
            return true;
        }

        private void _broadcastAboutToConnect()
        {
            if (OnAboutToConnect != null)
            {
                OnAboutToConnect.Invoke();
            }
        }

        private bool _isConnectTaskStillWorking()
        {
            if (_sessionConnectTask == null)
            {
                return false;
            }
            if (_sessionConnectTask.IsCanceled)
            {
                _melonMod.LoggerInstance.Error("FAILED TO CONNECT TO " + _prettyPrintSessionUri + ", RETRYING...");
                return false;
            }
            return !_sessionConnectTask.IsCompleted;
        }

        private void _startConnect()
        {
            _melonMod.LoggerInstance.Msg(ConsoleColor.Yellow, "CONNECTING TO " + _prettyPrintSessionUri + "...");
            _sessionConnectTask = _session.ConnectAsync();
        }

        private void _onSocketClosed(string reason)
        {
            _melonMod.LoggerInstance.Error("LOST CONNECTION TO MULTIWORLD SERVER: " + reason);
        }
        
        private void _onSocketError(Exception e, string message)
        {
            _melonMod.LoggerInstance.Error("LOST CONNECTION TO MULTIWORLD SERVER: " + message);
        }

        private Task<LoginResult> _sessionLoginTask = null;
        private bool _loggedInSuccessfully = false;

        private bool _loggingIn()
        {
            if (_loggedInSuccessfully)
            {
                return false;
            }
            if (_isLoginTaskStillWorking())
            {
                return true;
            }
            if (!_connectionConfigured || _loggedInSuccessfully)
            {
                return false;
            }
            _startLogin();
            return true;
        }

        private bool _isLoginTaskStillWorking()
        {
            if (_sessionLoginTask == null)
            {
                return false;
            }
            if (!_sessionLoginTask.IsCompleted)
            {
                return true;
            }
            LoginResult loginResult = _sessionLoginTask.Result;

            if (loginResult.Successful)
            {
                _loggedInSuccessfully = true;
                _broadcastLoginSuccess();
            }
            else
            {
                LoginFailure loginFailure = (LoginFailure)loginResult;
                _melonMod.LoggerInstance.Error("LOGIN FAILURE: " + string.Join(", ", loginFailure.Errors));
                _melonMod.LoggerInstance.Error("Please correct all errors then restart the game. No further login attempts will be made.");
                _connectionConfigured = false;
            }

            return false;
        }

        private void _resetLoginState()
        {
            _sessionLoginTask = null;
            _loggedInSuccessfully = false;
        }

        private void _broadcastLoginSuccess()
        {
            _melonMod.LoggerInstance.Msg(ConsoleColor.Green, "SUCCESS! LOGGED INTO " + _prettyPrintSessionUri + " AS " + _slotName);
            if (OnConnectionSuccessful != null)
            {
                OnConnectionSuccessful.Invoke(new ConnectionSuccessResponse(_slotName,
                    _session.Socket.Uri,
                    _session.DataStorage.GetSlotData(),
                    _session.Locations.AllLocationsChecked));
            }
        }

        private void _startLogin()
        {
            _melonMod.LoggerInstance.Msg(ConsoleColor.Yellow, "LOGGING IN AS " + _slotName + "...");
            _sessionLoginTask = _session.LoginAsync(GAME_NAME, _slotName, ItemsHandlingFlags.AllItems, version: new Version(AP_VERSION), password: _password, requestSlotData: true);
        }

        private bool _initConnectionInfoFromFile()
        {
            if (!File.Exists(CONNECTION_INI_PATH))
            {
                _melonMod.LoggerInstance.Warning(string.Format("CREATING CONNECTION CONFIG FILE: Configure your connection at {0}, then restart the game", CONNECTION_INI_PATH));
                _writeIniFileTemplate();
                return false;
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
                        return false;
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
                return false;
            }
            _slotName = slot;
            _password = password;
            try
            {
                _session = ArchipelagoSessionFactory.CreateSession(domain, (int)port);
            }
            catch (Exception ex)
            {
                _melonMod.LoggerInstance.Error("BAD CONNECTION CONFIG: " + ex.Message);
                return false;
            }
            _session.Items.ItemReceived += _onItemReceived;
            _session.Locations.CheckedLocationsUpdated += _onLocationClearedRemotely;
            _session.Socket.SocketClosed += _onSocketClosed;
            _session.Socket.ErrorReceived += _onSocketError;
            return true;
        }

        private void _onItemReceived(ReceivedItemsHelper helper)
        {
            if (OnItemReceived == null)
            {
                return;
            }

            ItemInfo item = helper.DequeueItem();
            while (item != null)
            {
                OnItemReceived.Invoke(item);
                item = helper.DequeueItem();
            }
        }

        private void _onLocationClearedRemotely(IReadOnlyList<long> newlyClearedLocations)
        {
            if (OnLocationClearedRemotely == null)
            {
                return;
            }
            foreach (long locationId in newlyClearedLocations)
            {
                OnLocationClearedRemotely.Invoke(locationId);
            }
        }

        private void _writeIniFileTemplate()
        {
            File.WriteAllLines(CONNECTION_INI_PATH, new string[]{
                "# The multiworld server domain",
                "domain=archipelago.gg",
                "# The multiworld port",
                "port=38281",
                "# The name of your player slot in the multiworld",
                "slot=DThaiPome_PMW2RP",
                "# The multiworld password (leave this blank if there is no password)",
                "password="
            });
        }

        public void ClearLocations(long[] locationIds)
        {
            if (!_loggedInSuccessfully)
            {
                return;
            }
            _session.Locations.CompleteLocationChecks(locationIds);
        }

        public void SetGoalAchieved()
        {
            if (!_loggedInSuccessfully)
            {
                return;
            }
            _session.SetGoalAchieved();
        }

        public struct ConnectionSuccessResponse : IAPSessionService.IConnectionSuccessResponse
        {
            public ConnectionSuccessResponse(string name, Uri uri, Dictionary<string, object> slotData, IReadOnlyList<long> locations)
            {
                SlotName = name;
                ConnectionUri = uri;
                SlotData = slotData;
                LocationsCleared = locations;
            }

            public string SlotName { get; private set; }
            public Uri ConnectionUri { get; private set; }

            public Dictionary<string, object> SlotData { get; private set; }

            public IReadOnlyList<long> LocationsCleared { get; private set; }
        }

        private string _prettyPrintSessionUri
        {
            get
            {
                return string.Format("{0}:{1}", _session.Socket.Uri.Host, _session.Socket.Uri.Port);
            }
        }
    }
}
