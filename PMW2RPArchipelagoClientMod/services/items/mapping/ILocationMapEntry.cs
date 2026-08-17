using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping
{
    public interface ILocationMapEntry
    {
        void ClearLocation(ILocationsSource locations);
    }
}
