using MelonLoader;
using PMW2RPArchipelagoClientMod.services.items.item.consumables;
using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.items.location.locations;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public class IdMapperService : IIdMapperService
    {
        private MelonMod _melonMod;

        private Dictionary<IdRange, Func<long, IUnlockableItemId>> _itemSuppliers = new Dictionary<IdRange, Func<long, IUnlockableItemId>>();
        private Dictionary<IdRange, Func<long, IUnlockableLocationId>> _locationSuppliers = new Dictionary<IdRange, Func<long, IUnlockableLocationId>>();
        private Dictionary<IdRange, Func<long, IUnlockableConsumableId>> _consumableSuppliers = new Dictionary<IdRange, Func<long, IUnlockableConsumableId>>();

        public IdMapperService(MelonMod melonMod)
        {
            _melonMod = melonMod;
        }

        public void Init()
        {
            _registerAll();
        }

        private void _registerAll()
        {
            FruitSwitchItem.Register();
            GoldenFruitItem.Register();
            MovesetItem.Register();
            PastKeyItem.Register();
            SkinItem.Register();
            StageItem.Register();

            CollectCapsuleLocation.Register();
            GalaxianCollectedLocation.Register();
            GoldMedalClearLocation.Register();
            MissionClearLocation.Register();
            StageClearLocation.Register();

            ExtraLifeItem.Register();
            PacDotBundle.Register();
            PointsBundle.Register();
            VoiceLineTrap.Register();
        }

        private bool _tryGetSupplierFromDict<T>(Dictionary<IdRange, Func<long, T>> dict, long id, out Func<long, T> supplier)
        {
            foreach (var key in dict.Keys)
            {
                if (id >= key.Min && id < key.Max)
                {
                    supplier = dict[key];
                    return true;
                }
            }
            supplier = null;
            return false;
        }

        public void RegisterItemInRange(long idGte, long idLt, Func<long, IUnlockableItemId> idToItem)
        {
            _registerEntryInRange(_itemSuppliers, idGte, idLt, idToItem);
        }

        public void RegisterLocationInRange(long idGte, long idLt, Func<long, IUnlockableLocationId> idToLocation)
        {
            _registerEntryInRange(_locationSuppliers, idGte, idLt, idToLocation);
        }

        private void _registerEntryInRange<T>(Dictionary<IdRange, Func<long, T>> dict, long idGte, long idLt, Func<long, T> supplier)
        {
            IdRange range = new IdRange();
            range.Min = idGte;
            range.Max = idLt;
            dict.Add(range, supplier);
        }

        public void RegisterConsumableInRange(long idGte, long idLt, Func<long, IUnlockableConsumableId> idToConsumable)
        {
            _registerEntryInRange(_consumableSuppliers, idGte, idLt, idToConsumable);
        }

        public bool TryGetItemFromId(long id, out IUnlockableItemId item)
        {
            if (_tryGetSupplierFromDict(_itemSuppliers, id, out var supplier))
            {
                item = supplier.Invoke(id);
                return true;
            }
            item = new UnknownItem(id);
            return false;
        }

        public bool TryGetLocationFromId(long id, out IUnlockableLocationId location)
        {
            if (_tryGetSupplierFromDict(_locationSuppliers, id, out var supplier))
            {
                location = supplier.Invoke(id);
                return true;
            }
            location = new UnknownLocation(id);
            return false;
        }

        public bool TryGetConsumableFromId(long id, out IUnlockableConsumableId consumable)
        {
            if (_tryGetSupplierFromDict(_consumableSuppliers, id, out var supplier))
            {
                consumable = supplier.Invoke(id);
                return true;
            }
            consumable = new UnknownConsumable(id);
            return false;
        }

        private struct IdRange
        {
            public long Min { get; set; }
            public long Max { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                IdRange range = (IdRange)obj;
                return range.Min == Min && range.Max == Max;
            }

            public override int GetHashCode()
            {
                return Min.GetHashCode() ^ Max.GetHashCode();
            }
        };
    }
}
