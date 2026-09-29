using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public interface IUnlocksSource
    {
        bool IsUnlocked(IUnlockableItemId item);
        int GetCountReceived(IUnlockableItemId item);
        void FlushConsumables();
        void GiveConsumableReceiver(IConsumableDispatcher dispatcher);
    }
}
