using PMW2RPArchipelagoClientMod.models.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class PastKeyItemResult : IItemMapEntry
    {
        private PastKeyItem _pastKey;

        public PastKeyItemResult(PastKeyItem pastKey)
        {
            _pastKey = pastKey;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.PastKeysMutable.Add(_pastKey);
        }
    }
}
