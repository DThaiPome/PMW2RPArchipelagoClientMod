using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;
using PMW2RPArchipelagoClientMod.util;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.LEVEL_OFFSET,
        exclusiveMax: WorldConstants.LEVEL_OFFSET + (long)PMW2RPConstants.STAGE_CAP + 1)]
    public class StageClearLocation : AUnlockableLocation
    {
        private static Dictionary<EWorldStage, StageClearLocation> STAGE_CLEAR_LOCATIONS;

        static StageClearLocation()
        {
            STAGE_CLEAR_LOCATIONS = new Dictionary<EWorldStage, StageClearLocation>();
        }

        public static StageClearLocation StageLocationFor(EWorldStage stage)
        {
            return _stageLocationFor(stage);
        }

        private static StageClearLocation _stageLocationFor(EWorldStage stage)
        {
            if (stage <= EWorldStage.PacVillage || stage > PMW2RPConstants.STAGE_CAP)
            {
                throw new InvalidDataException("bad stage location");
            }
            if (STAGE_CLEAR_LOCATIONS.TryGetValue(stage, out var location))
            {
                return location;
            }
            var newLocation = new StageClearLocation(stage, ServiceFactory.APConnectionService);
            STAGE_CLEAR_LOCATIONS.Add(stage, newLocation);
            return newLocation;
        }

        public static bool IsStageClear(ILocationsSource locations, EWorldStage stage)
        {
            return stage > EWorldStage.PacVillage && stage < PMW2RPConstants.STAGE_CAP && locations.IsCleared(_stageLocationFor(stage));
        }

        public static void ClearStage(ILocationsSource locations, EWorldStage stage)
        {
            if (stage <= EWorldStage.PacVillage || stage >= PMW2RPConstants.STAGE_CAP)
            {
                return;
            }
            locations.Clear(_stageLocationFor(stage));
        }

        private EWorldStage _stage;
        private IAPConnectionService _apConnectionService;

        public StageClearLocation(long id, IAPConnectionService apConnectionService) : base(id)
        {
            _stage = (EWorldStage)(id - 1 - WorldConstants.LEVEL_OFFSET);
            if (_stage < EWorldStage.PacVillage || _stage >= EWorldStage.StageSonic_1)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported stage clear ID: " + id);
            }

            _apConnectionService = apConnectionService;
        }

        public StageClearLocation(EWorldStage stage, IAPConnectionService apConnectionService) : base((long)stage + 1 + WorldConstants.LEVEL_OFFSET)
        {
            _stage = stage;
            _apConnectionService = apConnectionService;
        }

        public override void Clear(ILocationsDispatcher dispatcher)
        {
            GoalBossOption? goalBoss = _apConnectionService.GoalBoss;
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

        public EWorldStage Location => _stage;
    }
}
