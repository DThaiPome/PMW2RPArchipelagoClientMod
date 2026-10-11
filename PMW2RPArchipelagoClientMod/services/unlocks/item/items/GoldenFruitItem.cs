using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.GOLDEN_FRUIT_OFFSET,
        exclusiveMax: WorldConstants.GOLDEN_FRUIT_OFFSET + (long)GoldenFruitKind.MAX)]
    public class GoldenFruitItem : APermanentUnlockableItem<GoldenFruitKind>
    {

        public static Dictionary<GoldenFruitKind, GoldenFruitItem> GOLDEN_FRUIT_ITEMS { get; private set; }

        static GoldenFruitItem()
        {
            GOLDEN_FRUIT_ITEMS = new Dictionary<GoldenFruitKind, GoldenFruitItem>();
            for (var fruit = GoldenFruitKind.Cherry; fruit < GoldenFruitKind.MAX; fruit++)
            {
                GOLDEN_FRUIT_ITEMS[fruit] = new GoldenFruitItem(fruit);
            }
        }
        public static bool AreAllGoldenFruitsUnlocked(IUnlocksSource unlocks)
        {
            return GOLDEN_FRUIT_ITEMS.Values.All(unlocks.IsUnlocked);
        }

        public static bool IsGoldenFruitReceived(IUnlocksSource unlocks, GoldenFruitKind fruit)
        {
            return unlocks.IsUnlocked(GOLDEN_FRUIT_ITEMS[fruit]);
        }

        public static IEnumerable<GoldenFruitKind> GetReceivedGoldenFruits(IUnlocksSource unlocks)
        {
            return GOLDEN_FRUIT_ITEMS.Values.Where(unlocks.IsUnlocked).Select(item => item.Item).AsEnumerable();
        }

        private GoldenFruitKind _fruit;

        public GoldenFruitItem(long id) : base(id)
        {
            _fruit = (GoldenFruitKind)(id - WorldConstants.GOLDEN_FRUIT_OFFSET);
        }

        public GoldenFruitItem(GoldenFruitKind fruit) : base((long)fruit + WorldConstants.GOLDEN_FRUIT_OFFSET)
        {
            _fruit = fruit;
        }

        public override GoldenFruitKind Item => _fruit;
    }
}
