using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.location;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Rendering.GPUPrefixSum;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    public class GoldMedalClearLocation : AUnlockableLocation<EWorldStage>
    {
        private static readonly long TIMETRIAL_OFFSET = 1000;
        private static readonly long STAGE_ID_CEIL = TIMETRIAL_OFFSET + (long)EWorldStage.StageSonic_1 + 1;

        public static Dictionary<EWorldStage, GoldMedalClearLocation> GOLD_MEDAL_LOCATIONS { get; private set; }

        static GoldMedalClearLocation()
        {
            GOLD_MEDAL_LOCATIONS = new Dictionary<EWorldStage, GoldMedalClearLocation>();
            for (var stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                GOLD_MEDAL_LOCATIONS[stage] = new GoldMedalClearLocation(stage);
            }
        }

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(TIMETRIAL_OFFSET, STAGE_ID_CEIL, id => new GoldMedalClearLocation(id));
        }

        public static bool IsGoldMedalClear(ILocationsSource locations, EWorldStage stage)
        {
            return locations.IsCleared(GOLD_MEDAL_LOCATIONS[stage]);
        }

        public static void ClearGoldMedal(ILocationsSource locations, EWorldStage stage)
        {
            locations.Clear(GOLD_MEDAL_LOCATIONS[stage]);
        }

        private EWorldStage _stage;

        public GoldMedalClearLocation(long id) : base(id)
        {
            _stage = (EWorldStage)(id - 1 - TIMETRIAL_OFFSET);
            if (_stage < EWorldStage.PacVillage || _stage >= EWorldStage.StageSonic_1)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported gold medal ID: " + id);
            }
        }

        public GoldMedalClearLocation(EWorldStage stage) : base((long)stage + 1 + TIMETRIAL_OFFSET)
        {
            _stage = stage;
        }

        public override EWorldStage Location => _stage;
    }
}
