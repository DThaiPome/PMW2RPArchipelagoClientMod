namespace PMW2RPArchipelagoClientMod.services.items.item.items.@base
{
    public interface IUnlockableItemId
    {
        long Id { get; }

        void Unlock(IItemsDispatcher dispatcher);
    }
}
