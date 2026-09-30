namespace PMW2RPArchipelagoClientMod.services.items.location.locations.@base
{
    public interface IUnlockableLocation<T> : IUnlockableLocationId
    {
        T Location { get; }

        public bool Equals(IUnlockableLocation<T> other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return Id == other.Id;
        }
    }
}
