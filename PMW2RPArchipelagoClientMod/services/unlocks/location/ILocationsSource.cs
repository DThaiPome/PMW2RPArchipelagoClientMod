using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;

namespace PMW2RPArchipelagoClientMod.services.items.location
{
    public interface ILocationsSource
    {
        bool IsCleared(IUnlockableLocation location);
        void Clear(IUnlockableLocation location);
    }
}
