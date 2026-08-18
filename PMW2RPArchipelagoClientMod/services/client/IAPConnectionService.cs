using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Models;
using PMW2RPArchipelagoClientMod.models.data;
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
        void SendLocationChecked(long id);
        void SendLocationsChecked(long[] ids);
        void Goal();
        void OnLateUpdate();

        Action OnConnect { get; set; }
        Action<IReadOnlyList<ItemInfo>> InitItems { get; set; }
        Action<IReadOnlyList<long>> InitLocations { get; set; }
        Action<ItemInfo> ItemReceived { get; set; }
        Action<long> LocationCheckedRemotely {  get; set; }

        GoalBossOption? GoalBoss { get; }
    }
}
