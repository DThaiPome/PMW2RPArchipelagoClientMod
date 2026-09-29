using MelonLoader;
using PMW2RPArchipelagoClientMod.services.items.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.items.@base
{
    public abstract class APermanentUnlockableItem<T> : AUnlockableItem<T>
    {
        protected APermanentUnlockableItem(long id) : base(id)
        {
        }

        public override void Unlock(IItemsDispatcher dispatcher)
        {
            dispatcher.UnlockPermanently(this);
        }
    }
}
