using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;
using EPastKeyItem = PMW2RPArchipelagoClientMod.models.data.PastKeyKind;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.KEY_OFFSET,
        exclusiveMax: WorldConstants.KEY_OFFSET + (long)EPastKeyItem.MAX)]
    public class PastKeyItem : APermanentUnlockableItem<EPastKeyItem>
    {
        public static Dictionary<EPastKeyItem, PastKeyItem> PAST_KEY_ITEMS { get; private set; }

        static PastKeyItem()
        {
            PAST_KEY_ITEMS = new Dictionary<EPastKeyItem, PastKeyItem>();
            for (var key = EPastKeyItem.WindyWoodsKey; key < EPastKeyItem.MAX; key++)
            {
                PAST_KEY_ITEMS[key] = new PastKeyItem(key);
            }
        }

        public static bool AreAllKeysUnlocked(IUnlocksSource unlocks)
        {
            return PAST_KEY_ITEMS.Values.All(unlocks.IsUnlocked);
        }

        public static bool IsPastKeyReceived(IUnlocksSource unlocks, EPastKeyItem key)
        {
            return unlocks.IsUnlocked(PAST_KEY_ITEMS[key]);
        }

        public static IEnumerable<EPastKeyItem> GetPastKeysReceived(IUnlocksSource unlocks)
        {
            return PAST_KEY_ITEMS.Values.Where(unlocks.IsUnlocked).Select(item => item.Item).AsEnumerable();
        }

        private EPastKeyItem _key;

        public PastKeyItem(long id) : base(id)
        {
            _key = (EPastKeyItem)(id - WorldConstants.KEY_OFFSET);
        }

        public PastKeyItem(EPastKeyItem key) : base((long)key + WorldConstants.KEY_OFFSET)
        {
            _key = key;
        }

        public override EPastKeyItem Item => _key;
    }
}
