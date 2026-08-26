using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.locations
{
    public class CollectCapsuleLocationResult : ILocationMapEntry
    {
        private ECapsule _capsule;

        public CollectCapsuleLocationResult(ECapsule capsule)
        {
            _capsule = capsule;
        }

        public void ClearLocation(ILocationsSource locations)
        {
            locations.CollectCapsule(_capsule);
        }
    }
}
