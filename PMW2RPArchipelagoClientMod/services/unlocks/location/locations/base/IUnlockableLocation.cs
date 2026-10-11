namespace PMW2RPArchipelagoClientMod.services.items.location.locations.@base
{
    public interface IUnlockableLocation
    {
        long Id { get; }

        void Clear(ILocationsDispatcher dispatcher);
    }
}
