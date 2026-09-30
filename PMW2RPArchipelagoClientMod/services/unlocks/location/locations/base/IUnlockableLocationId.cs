namespace PMW2RPArchipelagoClientMod.services.items.location.locations.@base
{
    public interface IUnlockableLocationId
    {
        long Id { get; }

        void Clear(ILocationsDispatcher dispatcher);
    }
}
