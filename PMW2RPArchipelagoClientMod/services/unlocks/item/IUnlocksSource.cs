using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public interface IUnlocksSource
    {
        bool IsUnlocked(IUnlockableItemId item);
        int GetCountReceived(IUnlockableItemId item);
        IConsumablesDelegates ConsumablesDelegates { get; }
    }
}
