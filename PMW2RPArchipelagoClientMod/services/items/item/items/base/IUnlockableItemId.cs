using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMW2RPArchipelagoClientMod.services.items.item;

namespace PMW2RPArchipelagoClientMod.services.items.item.items.@base
{
    public interface IUnlockableItemId
    {
        long Id { get; }

        void Unlock(IItemsDispatcher dispatcher);
    }
}
