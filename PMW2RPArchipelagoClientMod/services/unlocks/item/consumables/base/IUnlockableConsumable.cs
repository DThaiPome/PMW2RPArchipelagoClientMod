namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public interface IUnlockableConsumable
    {
        long Id { get; }

        public void Consume(IConsumableDispatcher dispatcher);
    }
}
