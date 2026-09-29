using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMW2RPArchipelagoClientMod.services.items.v2.item;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.v2.consumable.consumables.@base
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
    }
}
