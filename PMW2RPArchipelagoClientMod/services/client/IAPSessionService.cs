using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.client
{
    public interface IAPSessionService
    {
        Action OnAboutToConnect { get; set; }
        Action<IConnectionSuccessResponse> OnConnectionSuccessful { get; set; }
        Action<ItemInfo> OnItemReceived { get; set; }
        Action<long> OnLocationClearedRemotely { get; set; }

        void OnLateUpdate();
        void ClearLocations(long[] locationIds);
        void SetGoalAchieved();

        public interface IConnectionSuccessResponse
        {
            string SlotName { get; }
            Uri ConnectionUri { get; }
            Dictionary<string, object> SlotData { get; }
            IReadOnlyList<long> LocationsCleared { get; }
        }
    }
}
