using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations
{
    public class StageClearedLocation : AUnlockableLocation<EWorldStage>
    {
        private static readonly long LEVEL_OFFSET = 0;
        private static readonly long STAGE_ID_CEIL = LEVEL_OFFSET + (long)EWorldStage.StageSonic_1 + 1;

        static StageClearedLocation()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(LEVEL_OFFSET, STAGE_ID_CEIL, id => new StageClearedLocation(id));
        }

        private EWorldStage _stage;

        public StageClearedLocation(long id) : base(id)
        {
            _stage = (EWorldStage)(id - 1 - LEVEL_OFFSET);
            if (_stage < EWorldStage.PacVillage || _stage >= EWorldStage.StageSonic_1)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported stage clear ID: " + id);
            }
        }

        public StageClearedLocation(EWorldStage stage) : base((long)stage + 1 + LEVEL_OFFSET)
        {
            _stage = stage;
        }

        public override EWorldStage Location => _stage;
    }
}
