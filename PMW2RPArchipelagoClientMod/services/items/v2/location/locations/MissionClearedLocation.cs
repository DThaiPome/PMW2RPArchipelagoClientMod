using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.location.locations.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations
{
    public class MissionClearedLocation : AUnlockableLocation<EMissionKind>
    {
        private static readonly long MISSION_OFFSET = 2000;
        private static readonly long MISSION_CEIL = MISSION_OFFSET + (long)EMissionKind.Mission100;

        static MissionClearedLocation()
        {
            ServiceFactory.IdMapperService.RegisterLocationInRange(MISSION_OFFSET, MISSION_CEIL, id => new MissionClearedLocation(id));
        }

        private EMissionKind _mission;

        public MissionClearedLocation(long id) : base(id)
        {
            _mission = (EMissionKind)(id - MISSION_OFFSET);
            if (_mission <= EMissionKind.None || _mission >= EMissionKind.Mission100)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported mission ID: " + id);
            }
        }

        public MissionClearedLocation(EMissionKind mission) : base((long)mission + MISSION_OFFSET)
        {
            _mission = mission;
        }

        public override EMissionKind Location => _mission;
    }
}
