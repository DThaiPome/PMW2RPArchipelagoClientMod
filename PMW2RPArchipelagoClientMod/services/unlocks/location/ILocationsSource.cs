using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;

namespace PMW2RPArchipelagoClientMod.services.items.location
{
    public interface ILocationsSource
    {
        bool IsCleared(IUnlockableLocation location);
        bool IsCleared(long id);
        void Clear(IUnlockableLocation location);
    }
}
