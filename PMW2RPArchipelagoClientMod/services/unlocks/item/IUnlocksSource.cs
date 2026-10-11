using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public interface IUnlocksSource
    {
        bool IsUnlocked(IUnlockableItem item);
        int GetCountReceived(IUnlockableItem item);
        IConsumablesDelegates ConsumablesDelegates { get; }
    }
}
