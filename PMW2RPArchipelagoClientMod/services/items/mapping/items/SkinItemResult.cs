using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class SkinItemResult : IItemMapEntry
    {
        private EPlayerSkin _skin;

        public SkinItemResult(EPlayerSkin skin)
        {
            _skin = skin;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.SkinsMutable.Add(_skin);
        }
    }
}
