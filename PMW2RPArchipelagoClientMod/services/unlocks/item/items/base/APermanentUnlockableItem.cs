namespace PMW2RPArchipelagoClientMod.services.items.item.items.@base
{
    public abstract class APermanentUnlockableItem : AUnlockableItem
    {
        protected APermanentUnlockableItem(long id) : base(id)
        {
        }

        public override void Unlock(IItemsDispatcher dispatcher)
        {
            dispatcher.UnlockPermanently(this);
        }
    }
}
