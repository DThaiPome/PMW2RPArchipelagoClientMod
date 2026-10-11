namespace PMW2RPArchipelagoClientMod.services.items.location.locations.@base
{
    public abstract class AUnlockableLocation : IUnlockableLocation
    {
        protected readonly long _id;
        public long Id => _id;

        protected AUnlockableLocation(long id)
        {
            _id = id;
        }

        public virtual void Clear(ILocationsDispatcher dispatcher)
        {
            dispatcher.ClearLocation(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is AUnlockableLocation other)) return false;

            return _id == other._id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
