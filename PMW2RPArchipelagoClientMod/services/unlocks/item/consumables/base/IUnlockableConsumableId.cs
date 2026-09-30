namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public interface IUnlockableConsumableId
    {
        long Id { get; }

        public void Consume(IConsumableDispatcher dispatcher);
    }
}
