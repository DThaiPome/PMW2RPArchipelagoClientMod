using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Rendering.GPUPrefixSum;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations
{
    public class GoldMedalClearedLocation : AUnlockableLocation<EWorldStage>
    {
        private static readonly long TIMETRIAL_OFFSET = 1000;
        private static readonly long STAGE_ID_CEIL = TIMETRIAL_OFFSET + (long)EWorldStage.StageSonic_1 + 1;

        static GoldMedalClearedLocation()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(TIMETRIAL_OFFSET, STAGE_ID_CEIL, id => new GoldMedalClearedLocation(id));
        }

        private EWorldStage _stage;

        public GoldMedalClearedLocation(long id) : base(id)
        {
            _stage = (EWorldStage)(id - 1 - TIMETRIAL_OFFSET);
            if (_stage < EWorldStage.PacVillage || _stage >= EWorldStage.StageSonic_1)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported gold medal ID: " + id);
            }
        }

        public GoldMedalClearedLocation(EWorldStage stage) : base((long)stage + 1 + TIMETRIAL_OFFSET)
        {
            _stage = stage;
        }

        public override EWorldStage Location => _stage;
    }
}
