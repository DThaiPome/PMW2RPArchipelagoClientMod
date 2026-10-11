using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;

namespace PMW2RPArchipelagoClientMod.services.unlocks.registry
{
    public interface IIdMapperService
    {
        void Init();
        void RegisterItemInRange(long idGte, long idLt, Func<long, IUnlockableItem> idToItem);
        void RegisterLocationInRange(long idGte, long idLt, Func<long, IUnlockableLocation> idToLocation);
        void RegisterConsumableInRange(long idGte, long idLt, Func<long, IUnlockableConsumable> idToConsumable);
        bool TryGetItemFromId(long id, out IUnlockableItem item);
        bool TryGetLocationFromId(long id, out IUnlockableLocation location);
        bool TryGetConsumableFromId(long id, out IUnlockableConsumable consumable);
    }
}
