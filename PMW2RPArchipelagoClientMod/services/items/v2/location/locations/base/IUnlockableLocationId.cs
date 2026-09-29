using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base
{
    public interface IUnlockableLocationId
    {
        long Id { get; }

        void Clear(ILocationsDispatcher dispatcher);
    }
}
