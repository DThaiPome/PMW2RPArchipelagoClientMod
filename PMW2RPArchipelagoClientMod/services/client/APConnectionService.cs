using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using MelonLoader;
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
        private List<IAPClientEventHandler> _clientEventHandlers;

        private static readonly long ITEMS_INIT_THRESHOLD_MS = 250;

        private long _lastConnectMs;
        private List<ItemInfo> _initItems = new List<ItemInfo>();

        public APConnectionService(MelonMod melonMod)
        {
            _melonMod = melonMod;
            _session = null;
            _clientEventHandlers = new List<IAPClientEventHandler>();
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
                _resetConnectStates();
                var session = ArchipelagoSessionFactory.CreateSession(domain, port);
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
        }

        private void _onLoginSuccess()
        {
            _melonMod.LoggerInstance.Msg("CONNECTED TO SERVER");
            foreach (var handler in _clientEventHandlers)
            {
                handler.OnConnect();
                handler.InitLocations(_session.Locations.AllLocationsChecked);
            }
        }

        private void _onItemReceived(ReceivedItemsHelper helper)
        {
            ItemInfo? item = helper.DequeueItem();
            while (item != null)
            {
                if (DateTime.Now.Millisecond - _lastConnectMs < ITEMS_INIT_THRESHOLD_MS)
                {
                    _initItems.Add(item);
                }
                else
                {
                    foreach (var handler in _clientEventHandlers)
                    {
                        handler.ItemReceived(item);
                    }
                }
                item = helper.DequeueItem();
            }
        }

        private void _onCheckedLocationsUpdated(ReadOnlyCollection<long> newCheckedLocations)
        {
            foreach (long id in newCheckedLocations)
            {
                foreach (var handler in _clientEventHandlers)
                {
                    handler.LocationCheckedRemotely(id);
                }
            }
        }

        public void HandleEvents(IAPClientEventHandler handler)
        {
            _clientEventHandlers.Add(handler);
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
            if (DateTime.Now.Millisecond - _lastConnectMs > ITEMS_INIT_THRESHOLD_MS && _initItems.Count > 0)
            {
                foreach (var handler in _clientEventHandlers)
                {
                    handler.InitItems(_initItems);
                }
                _initItems.Clear();
            }
        }
    }
}
