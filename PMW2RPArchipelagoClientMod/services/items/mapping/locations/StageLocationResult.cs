using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.locations
{
    public class StageLocationResult : ILocationMapEntry
    {
        private EWorldStage _stage;

        public StageLocationResult(EWorldStage stage)
        {
            _stage = stage;
        }

        public void ClearLocation(ILocationsSource locations)
        {
            locations.ClearStage(_stage);
        }
    }
}
