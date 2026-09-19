using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class FruitSwitchItemResult : IItemMapEntry
    {
        private EFruits _fruit;

        public FruitSwitchItemResult(EFruits fruit)
        {
            _fruit = fruit;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.FruitSwitchesMutable.Add(_fruit);
        }
    }
}
