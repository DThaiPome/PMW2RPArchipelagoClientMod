namespace PMW2RPArchipelagoClientMod.services.items.item.consumables.@base
{
    public abstract class AUnlockableConsumable : IUnlockableConsumable
    {
        protected long _id { get; private set; }
        public long Id => _id;

        public AUnlockableConsumable(long id)
        {
            _id = id;
        }

        public abstract void Consume(IConsumableDispatcher dispatcher);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is AUnlockableConsumable other)) return false;

            return _id == other._id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
