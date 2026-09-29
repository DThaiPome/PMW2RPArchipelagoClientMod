using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMW2RPArchipelagoClientMod.services.items.location;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations.@base
{
    public interface IUnlockableLocationId
    {
        long Id { get; }

        void Clear(ILocationsDispatcher dispatcher);
    }
}
