using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public interface IUnlocksSourceMutable : IUnlocksSource
    {
        void ReceiveItem(IUnlockableItemId item);
        void ReceiveConsumable(IUnlockableConsumableId consumableId);
        void RescindItem(IUnlockableItemId item);
    }
}
