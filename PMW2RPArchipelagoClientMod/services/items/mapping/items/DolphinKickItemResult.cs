using PMW2RPArchipelagoClientMod.models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class DolphinKickItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            if (unlocks.DolphinKick == ProgressiveDolphinKick.SuperDolphinKick)
            {
                return;
            }
            unlocks.DolphinKick++;
        }
    }
}
