using MelonLoader;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items.item.consumables;
using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.items.location.locations;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.util;
using System.Reflection;

namespace PMW2RPArchipelagoClientMod.services.unlocks.registry
{
    public class IdMapperService : IIdMapperService
    {
        private MelonMod _melonMod;
        private IAPConnectionService _apConnectionService;
        private IReflectionService _reflectionService;

        private Dictionary<IdRange, Func<long, IUnlockableItem>> _itemSuppliers = new Dictionary<IdRange, Func<long, IUnlockableItem>>();
        private Dictionary<IdRange, Func<long, IUnlockableLocation>> _locationSuppliers = new Dictionary<IdRange, Func<long, IUnlockableLocation>>();
        private Dictionary<IdRange, Func<long, IUnlockableConsumable>> _consumableSuppliers = new Dictionary<IdRange, Func<long, IUnlockableConsumable>>();

        private readonly Dictionary<Type, object> _injectionObjs;

        public IdMapperService(MelonMod melonMod,
            IAPConnectionService apConnectionService,
            IReflectionService reflectionService)
        {
            _melonMod = melonMod;
            _apConnectionService = apConnectionService;
            _reflectionService = reflectionService;

            _injectionObjs = new Dictionary<Type, object>
            {
                { typeof(IAPConnectionService), _apConnectionService }
            };
        }

        public void Init()
        {
            _registerSuppliers();
        }
        
        private void _registerSuppliers()
        {
            foreach (var type in _reflectionService.GetLoadedTypes())
            {
                MapFromIdRange attr = type.GetCustomAttribute<MapFromIdRange>();
                if (attr == null)
                {
                    continue;
                }
                long min = attr.InclusiveMin;
                long max = attr.ExclusiveMax;
                if (type.IsAssignableTo(typeof(IUnlockableItem)))
                {
                    _registerIfAble<IUnlockableItem>(type, min, max, RegisterItemInRange);
                }
                else if (type.IsAssignableTo(typeof(IUnlockableLocation)))
                {
                    _registerIfAble<IUnlockableLocation>(type, min, max, RegisterLocationInRange);
                }
                else if (type.IsAssignableTo(typeof(IUnlockableConsumable)))
                {
                    _registerIfAble<IUnlockableConsumable>(type, min, max, RegisterConsumableInRange);
                }
            }
        }

        private void _registerIfAble<T>(Type type, long min, long max, Action<long, long, Func<long, T>> register)
        {
            ConstructorInfo[] ctrs = type.GetConstructors();
            ConstructorInfo ctr = null;
            _findConstructorWithAnyOfTypes(ctrs, [typeof(long)], ref ctr);
            if (ctr == null)
            {
                _findConstructorWithAnyOfTypes(ctrs, _injectionObjs.Keys, ref ctr);
            }
            if (ctr == null)
            {
                ctr = type.GetConstructor([]);
            }
            if (ctr == null)
            {
                throw new InvalidDataException("no constructor for tagged class");
            }
            register.Invoke(min, max, id => _invokeMappedConstructor<T>(ctr, id));
        }

        private void _findConstructorWithAnyOfTypes(IEnumerable<ConstructorInfo> ctrs, IEnumerable<Type> paramTypes, ref ConstructorInfo ctr)
        {
            foreach (var ci in ctrs)
            {
                if (ci.GetParameters().Any(pi => paramTypes.Contains(pi.ParameterType)))
                {
                    ctr = ci;
                    return;
                }
            }
        }

        private T _invokeMappedConstructor<T>(ConstructorInfo ctr, long id)
        {
            return (T)ctr.Invoke(ctr.GetParameters().Select<ParameterInfo, object>(pi =>
            {
                if (pi.ParameterType == typeof(long))
                {
                    return id;
                }
                if (_injectionObjs.TryGetValue(pi.ParameterType, out object obj))
                {
                    return obj;
                }
                else
                {
                    // idk man just keep your constructors clean and everything will be fine
                    return null;
                }
            }).ToArray());
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

        public void RegisterItemInRange(long idGte, long idLt, Func<long, IUnlockableItem> idToItem)
        {
            _registerEntryInRange(_itemSuppliers, idGte, idLt, idToItem);
        }

        public void RegisterLocationInRange(long idGte, long idLt, Func<long, IUnlockableLocation> idToLocation)
        {
            _registerEntryInRange(_locationSuppliers, idGte, idLt, idToLocation);
        }

        private void _registerEntryInRange<T>(Dictionary<IdRange, Func<long, T>> dict, long idGte, long idLt, Func<long, T> supplier)
        {
            IdRange range = new IdRange();
            range.Min = idGte;
            range.Max = idLt;
            if (dict.ContainsKey(range))
            {
                _melonMod.LoggerInstance.Warning("DUPLICATE REGISTRY ENTRY FOR " + typeof(T).Name + ": " + range);
            }
            dict.Add(range, supplier);
        }

        public void RegisterConsumableInRange(long idGte, long idLt, Func<long, IUnlockableConsumable> idToConsumable)
        {
            _registerEntryInRange(_consumableSuppliers, idGte, idLt, idToConsumable);
        }

        public bool TryGetItemFromId(long id, out IUnlockableItem item)
        {
            if (_tryGetSupplierFromDict(_itemSuppliers, id, out var supplier))
            {
                item = supplier.Invoke(id);
                return true;
            }
            item = new UnknownItem(id);
            return false;
        }

        public bool TryGetLocationFromId(long id, out IUnlockableLocation location)
        {
            if (_tryGetSupplierFromDict(_locationSuppliers, id, out var supplier))
            {
                location = supplier.Invoke(id);
                return true;
            }
            location = new UnknownLocation(id);
            return false;
        }

        public bool TryGetConsumableFromId(long id, out IUnlockableConsumable consumable)
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
                return Min.GetHashCode() + Max.GetHashCode();
            }

            public override string ToString()
            {
                return string.Format("[{0},{1})", Min, Max);
            }
        };
    }
}
