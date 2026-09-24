using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.locations
{
    public class StageLocationResult : ILocationMapEntry
    {
        private EWorldStage _stage;

        public StageLocationResult(EWorldStage stage)
        {
            _stage = stage;
        }

        public void ClearLocation(ILocationsSource locations)
        {
            GoalBossOption? goalBoss = ServiceFactory.APConnectionService.GoalBoss;
            if ((goalBoss ?? GoalBossOption.Spooky) == GoalBossOption.Spooky && _stage == EWorldStage.Stage6_4)
            {
                return;
            }
            if ((goalBoss ?? GoalBossOption.TocMan) == GoalBossOption.TocMan && _stage == EWorldStage.Stage6_5)
            {
                return;
            }
            locations.ClearStage(_stage);
        }
    }
}
