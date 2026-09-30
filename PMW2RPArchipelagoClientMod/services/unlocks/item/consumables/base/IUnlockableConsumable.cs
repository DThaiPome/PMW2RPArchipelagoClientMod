namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public interface IUnlockableConsumable<T> : IUnlockableConsumableId
    {
        T Consumable { get; }
    }
}
