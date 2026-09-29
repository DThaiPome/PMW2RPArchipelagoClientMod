using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base
{
    public interface IUnlockableItemId
    {
        long Id { get; }

        void Unlock(IItemsDispatcher dispatcher);
    }
}
