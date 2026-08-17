using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.locations
{
    public class UnknownLocationResult : ILocationMapEntry
    {
        private long _id;

        public UnknownLocationResult(long id)
        {
            ServiceFactory.ModInstance.LoggerInstance.Msg("Found unknown location ID: " + id);
            _id = id;
        }

        public void ClearLocation(ILocationsSource locations)
        {
            ServiceFactory.ModInstance.LoggerInstance.Msg("Tried to clear unknown location ID: " + _id);
        }
    }
}
