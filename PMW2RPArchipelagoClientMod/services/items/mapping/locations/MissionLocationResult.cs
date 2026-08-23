using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.locations
{
    public class MissionLocationResult : ILocationMapEntry
    {
        private EMissionKind _kind;
        
        public MissionLocationResult(EMissionKind kind)
        {
            _kind = kind;
        }

        public void ClearLocation(ILocationsSource locations)
        {
            locations.ClearMission(_kind);
        }
    }
}
