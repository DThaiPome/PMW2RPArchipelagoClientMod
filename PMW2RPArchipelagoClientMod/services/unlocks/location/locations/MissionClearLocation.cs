using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.location.locations.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.location.locations
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.MISSION_OFFSET,
        exclusiveMax: WorldConstants.MISSION_OFFSET + (long)EMissionKind.Mission100)]
    public class MissionClearLocation : AUnlockableLocation<EMissionKind>
    {
        public static Dictionary<EMissionKind, MissionClearLocation> MISSION_LOCATIONS { get; private set; }

        static MissionClearLocation()
        {
            MISSION_LOCATIONS = new Dictionary<EMissionKind, MissionClearLocation>();
            for (var mission = EMissionKind.Mission1; mission < EMissionKind.Mission100; mission++)
            {
                MISSION_LOCATIONS[mission] = new MissionClearLocation(mission);
            }
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
            _mission = (EMissionKind)(id - WorldConstants.MISSION_OFFSET);
            if (_mission <= EMissionKind.None || _mission >= EMissionKind.Mission100)
            {
                ServiceFactory.ModInstance.LoggerInstance.Warning("Parsed unsupported mission ID: " + id);
            }
        }

        public MissionClearLocation(EMissionKind mission) : base((long)mission + WorldConstants.MISSION_OFFSET)
        {
            _mission = mission;
        }

        public override EMissionKind Location => _mission;
    }
}
