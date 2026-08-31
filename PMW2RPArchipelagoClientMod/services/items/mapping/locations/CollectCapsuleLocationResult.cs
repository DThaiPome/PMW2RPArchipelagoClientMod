using Il2Cpp;

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
