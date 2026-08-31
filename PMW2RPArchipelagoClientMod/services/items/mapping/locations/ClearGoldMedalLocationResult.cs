using Il2Cpp;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.locations
{
    public class ClearGoldMedalLocationResult : ILocationMapEntry
    {
        private EWorldStage _stage;

        public ClearGoldMedalLocationResult(EWorldStage stage)
        {
            _stage = stage;
        }

        public void ClearLocation(ILocationsSource locations)
        {
            locations.ClearGoldMedal(_stage);
        }
    }
}
