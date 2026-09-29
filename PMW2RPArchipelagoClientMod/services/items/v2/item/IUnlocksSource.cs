using PMW2RPArchipelagoClientMod.services.items.v2.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item
{
    public interface IUnlocksSource
    {
        bool IsUnlocked(IUnlockableItemId item);
        int GetCountReceived(IUnlockableItemId item);
        void FlushConsumables();
        void GiveConsumableReceiver(IConsumableDispatcher dispatcher);
    }
}
