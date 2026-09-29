using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.items
{
    public class FruitSwitchItem : APermanentUnlockableItem<EFruits>
    {
        private static readonly long FRUIT_SWITCH_OFFSET = 400;
        private static readonly long FRUIT_SWITCH_CEIL = FRUIT_SWITCH_OFFSET + (long)EFruits.MAX;

        static FruitSwitchItem()
        {
            ServiceFactory.IdMapperService.RegisterItemInRange(FRUIT_SWITCH_OFFSET, FRUIT_SWITCH_CEIL, id => new FruitSwitchItem(id));
        }

        private EFruits _fruit;

        public FruitSwitchItem(long id) : base(id)
        {
            _fruit = (EFruits)(id -  FRUIT_SWITCH_OFFSET);
        }

        public FruitSwitchItem(EFruits fruit) : base((long)fruit + FRUIT_SWITCH_OFFSET)
        {
            _fruit = fruit;
        }

        public override EFruits Item => _fruit;
    }
}
