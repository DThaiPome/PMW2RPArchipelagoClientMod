using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.LEVEL_OFFSET,
        exclusiveMax: WorldConstants.LEVEL_OFFSET + (long)EWorldStage.StageSonic_1 + 1)]
    public class StageClearLocation : AUnlockableLocation<EWorldStage>
    {
        public static Dictionary<EWorldStage, StageClearLocation> STAGE_CLEAR_LOCATIONS { get; private set; }

        static StageClearLocation()
        {
            STAGE_CLEAR_LOCATIONS = new Dictionary<EWorldStage, StageClearLocation>();
            for (var stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                STAGE_CLEAR_LOCATIONS[stage] = new StageClearLocation(stage);
            }
        }

        public static bool IsStageClear(ILocationsSource locations, EWorldStage stage)
        {
            return locations.IsCleared(STAGE_CLEAR_LOCATIONS[stage]);
        }

        public static void ClearStage(ILocationsSource locations, EWorldStage stage)
        {
            locations.Clear(STAGE_CLEAR_LOCATIONS[stage]);
        }

        private EWorldStage _stage;

        public StageClearLocation(long id) : base(id)
        {
            _stage = (EWorldStage)(id - 1 - WorldConstants.LEVEL_OFFSET);
            if (_stage < EWorldStage.PacVillage || _stage >= EWorldStage.StageSonic_1)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported stage clear ID: " + id);
            }
        }

        public StageClearLocation(EWorldStage stage) : base((long)stage + 1 + WorldConstants.LEVEL_OFFSET)
        {
            _stage = stage;
        }

        public override void Clear(ILocationsDispatcher dispatcher)
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
            base.Clear(dispatcher);
        }

        public override EWorldStage Location => _stage;
    }
}
