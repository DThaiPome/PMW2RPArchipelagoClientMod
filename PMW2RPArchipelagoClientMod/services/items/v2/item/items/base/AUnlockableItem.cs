using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base
{
    public abstract class AUnlockableItem<T> : IUnlockableItem<T>
    {
        protected readonly long _id;
        public long Id => _id;

        public abstract T Item { get; }

        protected AUnlockableItem(long id)
        {
            _id = id;
        }

        public abstract void Unlock(IItemsDispatcher dispatcher);
    }
}
