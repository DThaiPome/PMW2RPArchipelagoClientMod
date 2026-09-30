using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    public class UnknownItem : AUnlockableItem<long>
    {
        public UnknownItem(long id) : base(id)
        {
        }

        public override long Item => Id;

        public override void Unlock(IItemsDispatcher dispatcher)
        {
            ServiceFactory.ModInstance.LoggerInstance.Warning("Unlocking unknown item with id: " + Id);
        }
    }
}
