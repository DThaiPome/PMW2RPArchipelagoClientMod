using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class StageItemResult : IItemMapEntry
    {
        private EWorldStage _stage;

        public StageItemResult(EWorldStage stage)
        {
            _stage = stage;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.StagesMutable[_stage] = true;
        }
    }
}
