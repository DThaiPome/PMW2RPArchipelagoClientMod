using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base
{
    public interface IUnlockableLocation<T> : IUnlockableLocationId
    {
        T Location { get; }

        public bool Equals(IUnlockableLocation<T> other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return Id == other.Id;
        }

        bool Equals<R>(IUnlockableLocation<R> other)
        {
            if (other == null) return false;
            if (typeof(T) == typeof(R)) return false;
            return Equals((IUnlockableLocation<T>)other);
        }

        new int GetHashCode()
        {
            return typeof(T).GetHashCode() + Id.GetHashCode();
        }
    }
}
