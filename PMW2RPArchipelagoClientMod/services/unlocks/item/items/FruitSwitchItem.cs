using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.FRUIT_SWITCH_OFFSET,
        exclusiveMax: WorldConstants.FRUIT_SWITCH_OFFSET + (long)EFruits.MAX)]
    public class FruitSwitchItem : APermanentUnlockableItem<EFruits>
    {
        public static Dictionary<EFruits, FruitSwitchItem> FRUIT_SWITCH_ITEMS { get; private set; }

        static FruitSwitchItem()
        {
            FRUIT_SWITCH_ITEMS = new Dictionary<EFruits, FruitSwitchItem>();
            for (var fruit = EFruits.Cherry; fruit < EFruits.MAX; fruit++)
            {
                FRUIT_SWITCH_ITEMS[fruit] = new FruitSwitchItem(fruit);
            }
        }

        public static bool IsFruitSwitchReceived(IUnlocksSource unlocks, EFruits fruit)
        {
            return unlocks.IsUnlocked(FRUIT_SWITCH_ITEMS[fruit]);
        }

        private EFruits _fruit;

        public FruitSwitchItem(long id) : base(id)
        {
            _fruit = (EFruits)(id - WorldConstants.FRUIT_SWITCH_OFFSET);
        }

        public FruitSwitchItem(EFruits fruit) : base((long)fruit + WorldConstants.FRUIT_SWITCH_OFFSET)
        {
            _fruit = fruit;
        }

        public override EFruits Item => _fruit;
    }
}
