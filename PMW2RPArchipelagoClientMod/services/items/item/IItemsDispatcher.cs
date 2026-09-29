using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public interface IItemsDispatcher
    {
        void UnlockPermanently(IUnlockableItemId item);
    }
}
