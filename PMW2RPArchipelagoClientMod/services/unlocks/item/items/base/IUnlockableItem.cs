namespace PMW2RPArchipelagoClientMod.services.items.item.items.@base
{
    public interface IUnlockableItem
    {
        long Id { get; }

        void Unlock(IItemsDispatcher dispatcher);
    }
}
