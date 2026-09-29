using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.location;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    public class MissionClearLocation : AUnlockableLocation<EMissionKind>
    {
        private static readonly long MISSION_OFFSET = 2000;
        private static readonly long MISSION_CEIL = MISSION_OFFSET + (long)EMissionKind.Mission100;

        public static Dictionary<EMissionKind, MissionClearLocation> MISSION_LOCATIONS { get; private set; }

        static MissionClearLocation()
        {
            MISSION_LOCATIONS = new Dictionary<EMissionKind, MissionClearLocation>();
            for (var mission = EMissionKind.Mission1; mission < EMissionKind.Mission100; mission++)
            {
                MISSION_LOCATIONS[mission] = new MissionClearLocation(mission);
            }
        }

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(MISSION_OFFSET, MISSION_CEIL, id => new MissionClearLocation(id));
        }

        public static bool IsMissionClear(ILocationsSource locations, EMissionKind mission)
        {
            return locations.IsCleared(MISSION_LOCATIONS[mission]);
        }

        public static void ClearMission(ILocationsSource locations, EMissionKind mission)
        {
            locations.Clear(MISSION_LOCATIONS[mission]);
        }

        private EMissionKind _mission;

        public MissionClearLocation(long id) : base(id)
        {
            _mission = (EMissionKind)(id - MISSION_OFFSET);
            if (_mission <= EMissionKind.None || _mission >= EMissionKind.Mission100)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported mission ID: " + id);
            }
        }

        public MissionClearLocation(EMissionKind mission) : base((long)mission + MISSION_OFFSET)
        {
            _mission = mission;
        }

        public override EMissionKind Location => _mission;
    }
}
