using Archipelago.MultiClient.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.client
{
    public interface IAPConnectionService
    {
        bool CreateSessionAndLogIn(string domain, int port, string slotName, string password = null);
        bool CloseSession();
        void HandleEvents(IAPClientEventHandler handler);
        void SendLocationChecked(long id);
        void SendLocationsChecked(long[] ids);
        void OnLateUpdate();
    }
}
