namespace PMW2RPArchipelagoClientMod.services.items.item.items.@base
{
    public interface IUnlockableItem<T> : IUnlockableItemId
    {
        T Item { get; }
    }
}
