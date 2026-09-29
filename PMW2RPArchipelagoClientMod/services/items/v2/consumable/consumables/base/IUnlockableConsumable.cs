using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.consumable.consumables.@base
{
    public interface IUnlockableConsumable<T> : IUnlockableConsumableId
    {
        T Consumable { get; }

        public bool Equals(IUnlockableConsumable<T> other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return Id == other.Id;
        }

        bool Equals<R>(IUnlockableConsumable<R> other)
        {
            if (other == null) return false;
            if (typeof(T) == typeof(R)) return false;
            return Equals((IUnlockableItem<T>)other);
        }

        new int GetHashCode()
        {
            return typeof(T).GetHashCode() + Id.GetHashCode();
        }
    }
}
