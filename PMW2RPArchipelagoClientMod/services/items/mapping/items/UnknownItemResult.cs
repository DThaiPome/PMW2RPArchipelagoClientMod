using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class UnknownItemResult : IItemMapEntry
    {
        private long _id;

        public UnknownItemResult(long id)
        {
            ServiceFactory.ModInstance.LoggerInstance.Msg("Found unknown item ID: " + id);
            _id = id;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            ServiceFactory.ModInstance.LoggerInstance.Msg("Tried to unlock unknown item ID: " + _id);
        }
    }
}
