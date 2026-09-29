using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.location;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    public class CollectCapsuleLocation : AUnlockableLocation<ECapsule>
    {
        private static readonly long GASHAPON_OFFSET = 3000;
        private static readonly long CAPSULE_CEIL = GASHAPON_OFFSET + (long)ECapsule.Capsule54;

        public static Dictionary<ECapsule, CollectCapsuleLocation> CAPSULE_LOCATIONS { get; private set; }

        static CollectCapsuleLocation()
        {

            CAPSULE_LOCATIONS = new Dictionary<ECapsule, CollectCapsuleLocation>();
            for (var capsule = ECapsule.Capsule1; capsule < ECapsule.Capsule54; capsule++)
            {
                CAPSULE_LOCATIONS[capsule] = new CollectCapsuleLocation(capsule);
            }
        }

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(GASHAPON_OFFSET, CAPSULE_CEIL, id => new CollectCapsuleLocation(id));
        }

        public static bool IsCapsuleCollected(ILocationsSource locations, ECapsule capsule)
        {
            return locations.IsCleared(CAPSULE_LOCATIONS[capsule]);
        }

        public static void ClearCapsuleCollected(ILocationsSource locations, ECapsule capsule)
        {
            locations.Clear(CAPSULE_LOCATIONS[capsule]);
        }

        private ECapsule _capsule;

        public CollectCapsuleLocation(long id) : base(id)
        {
            _capsule = (ECapsule)(id - GASHAPON_OFFSET);
            if (_capsule <= ECapsule.None || _capsule >= ECapsule.Capsule54)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported capsule ID: " + id);
            }
        }

        public CollectCapsuleLocation(ECapsule capsule) : base((long)capsule + GASHAPON_OFFSET)
        {
            _capsule = capsule;
        }

        public override ECapsule Location => _capsule;
    }
}
