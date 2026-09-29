using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public interface IIdMapperService
    {
        void Init();
        void RegisterItemInRange(long idGte, long idLt, Func<long, IUnlockableItemId> idToItem);
        void RegisterLocationInRange(long idGte, long idLt, Func<long, IUnlockableLocationId> idToLocation);
        void RegisterConsumableInRange(long idGte, long idLt, Func<long, IUnlockableConsumableId> idToConsumable);
        bool TryGetItemFromId(long id, out IUnlockableItemId item);
        bool TryGetLocationFromId(long id, out IUnlockableLocationId location);
        bool TryGetConsumableFromId(long id, out IUnlockableConsumableId consumable);
    }
}
