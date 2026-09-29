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

        public bool Equals(IUnlockableLocationId other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return Id == other.Id;
        }

        int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
