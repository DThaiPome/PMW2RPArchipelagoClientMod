using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.GOLDEN_FRUIT_OFFSET,
        exclusiveMax: WorldConstants.GOLDEN_FRUIT_OFFSET + (long)EFruits.MAX)]
    public class GoldenFruitItem : APermanentUnlockableItem<EFruits>
    {

        public static Dictionary<EFruits, GoldenFruitItem> GOLDEN_FRUIT_ITEMS { get; private set; }

        static GoldenFruitItem()
        {
            GOLDEN_FRUIT_ITEMS = new Dictionary<EFruits, GoldenFruitItem>();
            for (var fruit = EFruits.Cherry; fruit < EFruits.MAX; fruit++)
            {
                GOLDEN_FRUIT_ITEMS[fruit] = new GoldenFruitItem(fruit);
            }
        }
        public static bool AreAllGoldenFruitsUnlocked(IUnlocksSource unlocks)
        {
            return GOLDEN_FRUIT_ITEMS.Values.All(unlocks.IsUnlocked);
        }

        public static bool IsGoldenFruitReceived(IUnlocksSource unlocks, EFruits fruit)
        {
            return unlocks.IsUnlocked(GOLDEN_FRUIT_ITEMS[fruit]);
        }

        public static IEnumerable<EFruits> GetReceivedGoldenFruits(IUnlocksSource unlocks)
        {
            return GOLDEN_FRUIT_ITEMS.Values.Where(unlocks.IsUnlocked).Select(item => item.Item).AsEnumerable();
        }

        private EFruits _fruit;

        public GoldenFruitItem(long id) : base(id)
        {
            _fruit = (EFruits)(id - WorldConstants.GOLDEN_FRUIT_OFFSET);
        }

        public GoldenFruitItem(EFruits fruit) : base((long)fruit + WorldConstants.GOLDEN_FRUIT_OFFSET)
        {
            _fruit = fruit;
        }

        public override EFruits Item => _fruit;
    }
}
