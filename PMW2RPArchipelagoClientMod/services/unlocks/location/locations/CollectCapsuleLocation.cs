using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.GASHAPON_OFFSET,
        exclusiveMax: WorldConstants.GASHAPON_OFFSET + (long)CAPSULE_CAP)]
    public class CollectCapsuleLocation : AUnlockableLocation
    {
        public const ECapsule CAPSULE_CAP = ECapsule.Capsule55;

        public static Dictionary<ECapsule, CollectCapsuleLocation> CAPSULE_LOCATIONS { get; private set; }

        static CollectCapsuleLocation()
        {
            CAPSULE_LOCATIONS = new Dictionary<ECapsule, CollectCapsuleLocation>();
            for (var capsule = ECapsule.Capsule1; capsule < CAPSULE_CAP; capsule++)
            {
                CAPSULE_LOCATIONS[capsule] = new CollectCapsuleLocation(capsule);
            }
        }

        public static bool IsCapsuleCollected(ILocationsSource locations, ECapsule capsule)
        {
            return capsule > ECapsule.None && capsule < CAPSULE_CAP && locations.IsCleared(CAPSULE_LOCATIONS[capsule]);
        }

        public static void ClearCapsuleCollected(ILocationsSource locations, ECapsule capsule)
        {
            if (capsule <= ECapsule.None || capsule >= CAPSULE_CAP)
            {
                return;
            }
            locations.Clear(CAPSULE_LOCATIONS[capsule]);
        }

        private ECapsule _capsule;

        public CollectCapsuleLocation(long id) : base(id)
        {
            _capsule = (ECapsule)(id - WorldConstants.GASHAPON_OFFSET);
            if (_capsule <= ECapsule.None || _capsule >= CAPSULE_CAP)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported capsule ID: " + id);
            }
        }

        public CollectCapsuleLocation(ECapsule capsule) : base((long)capsule + WorldConstants.GASHAPON_OFFSET)
        {
            _capsule = capsule;
        }

        public ECapsule Location => _capsule;
    }
}
