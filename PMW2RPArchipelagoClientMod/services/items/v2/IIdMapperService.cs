using PMW2RPArchipelagoClientMod.services.items.v2.consumable.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2
{
    public interface IIdMapperService
    {
        void RegisterItemInRange(long idGte, long idLt, Func<long, IUnlockableItemId> idToItem);
        void RegisterLocationInRange(long idGte, long idLt, Func<long, IUnlockableLocationId> idToLocation);
        void RegisterConsumableInRange(long idGte, long idLt, Func<long, IUnlockableConsumableId> idToConsumable);
        bool TryGetItemFromId(long id, out IUnlockableItemId item);
        bool TryGetLocationFromId(long id, out IUnlockableLocationId location);
        bool TryGetConsumableFromId(long id, out IUnlockableConsumableId consumable);
    }
}
