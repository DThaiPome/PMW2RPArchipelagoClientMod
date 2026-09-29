using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.item;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGoldenFruitItem = Il2Cpp.EFruits;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    public class GoldenFruitItem : APermanentUnlockableItem<EGoldenFruitItem>
    {
        private static readonly long GOLDEN_FRUIT_OFFSET = 100;
        private static readonly long GOLDEN_FRUIT_CEIL = GOLDEN_FRUIT_OFFSET + (long)EGoldenFruitItem.MAX;

        public static Dictionary<EGoldenFruitItem, GoldenFruitItem> GOLDEN_FRUIT_ITEMS { get; private set; }

        static GoldenFruitItem()
        {
            GOLDEN_FRUIT_ITEMS = new Dictionary<EGoldenFruitItem, GoldenFruitItem>();
            for (var fruit = EGoldenFruitItem.Cherry; fruit < EGoldenFruitItem.MAX; fruit++)
            {
                GOLDEN_FRUIT_ITEMS[fruit] = new GoldenFruitItem(fruit);
            }
        }

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterItemInRange(GOLDEN_FRUIT_OFFSET, GOLDEN_FRUIT_CEIL, id => new GoldenFruitItem(id));
        }

        public static bool AreAllGoldenFruitsUnlocked(IUnlocksSource unlocks)
        {
            return GOLDEN_FRUIT_ITEMS.Values.All(unlocks.IsUnlocked);
        }

        public static bool IsGoldenFruitReceived(IUnlocksSource unlocks, EGoldenFruitItem fruit)
        {
            return unlocks.IsUnlocked(GOLDEN_FRUIT_ITEMS[fruit]);
        }

        public static IEnumerable<EGoldenFruitItem> GetReceivedGoldenFruits(IUnlocksSource unlocks)
        {
            return GOLDEN_FRUIT_ITEMS.Values.Where(unlocks.IsUnlocked).Select(item => item.Item).AsEnumerable();
        }

        private EGoldenFruitItem _fruit;

        public GoldenFruitItem(long id) : base(id)
        {
            _fruit = (EGoldenFruitItem)(id - GOLDEN_FRUIT_OFFSET);
        }

        public GoldenFruitItem(EGoldenFruitItem fruit) : base((long)fruit + GOLDEN_FRUIT_OFFSET)
        {
            _fruit = fruit;
        }

        public override EGoldenFruitItem Item => _fruit;
    }
}
