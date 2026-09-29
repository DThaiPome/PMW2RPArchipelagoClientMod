using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.location
{
    public interface ILocationsSource
    {
        bool IsCleared(IUnlockableLocationId location);
        void Clear(IUnlockableLocationId location);
    }
}
