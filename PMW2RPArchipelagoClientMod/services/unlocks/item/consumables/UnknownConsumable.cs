using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item.consumables
{
    public class UnknownConsumable : AUnlockableConsumable<long>
    {
        public UnknownConsumable(long id) : base(id)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unknown consumable with ID: " + id);
        }

        public override long Consumable => Id;

        public override void Consume(IConsumableDispatcher dispatcher)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Tried to use unknown consumable with ID: " + Id);
        }
    }
}
