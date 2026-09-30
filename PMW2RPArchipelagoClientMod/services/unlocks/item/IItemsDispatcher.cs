using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public interface IItemsDispatcher
    {
        void UnlockPermanently(IUnlockableItemId item);
    }
}
