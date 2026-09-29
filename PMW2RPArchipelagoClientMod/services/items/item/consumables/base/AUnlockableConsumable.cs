using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public abstract class AUnlockableConsumable<T> : IUnlockableConsumable<T>
    {
        protected long _id { get; private set; }
        public abstract T Consumable { get; }
        public long Id => _id;

        public AUnlockableConsumable(long id)
        {
            _id = id;
        }

        public abstract void Consume(IConsumableDispatcher dispatcher);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is AUnlockableConsumable<T> other)) return false;

            return _id == other._id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
