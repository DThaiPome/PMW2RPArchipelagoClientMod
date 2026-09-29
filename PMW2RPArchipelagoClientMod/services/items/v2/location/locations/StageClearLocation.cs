using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations
{
    public class StageClearLocation : AUnlockableLocation<EWorldStage>
    {
        private static readonly long LEVEL_OFFSET = 0;
        private static readonly long STAGE_ID_CEIL = LEVEL_OFFSET + (long)EWorldStage.StageSonic_1 + 1;

        public static Dictionary<EWorldStage, StageClearLocation> STAGE_CLEAR_LOCATIONS { get; private set; }

        static StageClearLocation()
        {
            STAGE_CLEAR_LOCATIONS = new Dictionary<EWorldStage, StageClearLocation>();
            for (var stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                STAGE_CLEAR_LOCATIONS[stage] = new StageClearLocation(stage);
            }
        }

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(LEVEL_OFFSET, STAGE_ID_CEIL, id => new StageClearLocation(id));
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
            _stage = (EWorldStage)(id - 1 - LEVEL_OFFSET);
            if (_stage < EWorldStage.PacVillage || _stage >= EWorldStage.StageSonic_1)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported stage clear ID: " + id);
            }
        }

        public StageClearLocation(EWorldStage stage) : base((long)stage + 1 + LEVEL_OFFSET)
        {
            _stage = stage;
        }

        public override EWorldStage Location => _stage;
    }
}
