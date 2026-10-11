using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public interface IUnlocksSourceMutable : IUnlocksSource
    {
        void ReceiveItem(IUnlockableItem item);
        void ReceiveConsumable(IUnlockableConsumable consumableId);
        void RescindItem(IUnlockableItem item);
    }
}
