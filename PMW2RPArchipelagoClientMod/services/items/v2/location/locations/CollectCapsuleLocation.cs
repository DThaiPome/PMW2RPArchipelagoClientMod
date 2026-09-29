using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations
{
    public class CollectCapsuleLocation : AUnlockableLocation<ECapsule>
    {
        private static readonly long GASHAPON_OFFSET = 3000;
        private static readonly long CAPSULE_CEIL = GASHAPON_OFFSET + (long)ECapsule.Capsule54;

        static CollectCapsuleLocation()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(GASHAPON_OFFSET, CAPSULE_CEIL, id => new CollectCapsuleLocation(id));
        }

        private ECapsule _capsule;

        public CollectCapsuleLocation(long id) : base(id)
        {
            _capsule = (ECapsule)(id -  GASHAPON_OFFSET);
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
