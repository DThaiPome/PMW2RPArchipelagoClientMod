using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations
{
    public class GalaxianCollectedLocation : AUnlockableLocation<EWorldStage>
    {
        private static readonly long GALAXIAN_OFFSET = 4000;
        private static readonly long STAGE_ID_CEIL = GALAXIAN_OFFSET + (long)EWorldStage.StageSonic_1 + 1;

        static GalaxianCollectedLocation()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(GALAXIAN_OFFSET, STAGE_ID_CEIL, id => new GalaxianCollectedLocation(id));
        }

        private EWorldStage _stage;

        public GalaxianCollectedLocation(long id) : base(id)
        {
            _stage = (EWorldStage)(id - 1 - GALAXIAN_OFFSET);
            if (_stage < EWorldStage.PacVillage || _stage >= EWorldStage.StageSonic_1)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported galaxian ID: " + id);
            }
        }

        public GalaxianCollectedLocation(EWorldStage stage) : base((long)stage + 1 + GALAXIAN_OFFSET)
        {
            _stage = stage;
        }

        public override EWorldStage Location => _stage;
    }
}
