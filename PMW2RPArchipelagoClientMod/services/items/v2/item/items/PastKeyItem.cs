using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPastKeyItem = PMW2RPArchipelagoClientMod.models.data.PastKeyKind;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.items
{
    public class PastKeyItem : APermanentUnlockableItem<EPastKeyItem>
    {
        private static readonly long KEY_OFFSET = 200;
        private static readonly long KEY_CEIL = KEY_OFFSET + (long)EPastKeyItem.MAX;

        public static Dictionary<EPastKeyItem, PastKeyItem> PAST_KEY_ITEMS { get; private set; }

        static PastKeyItem()
        {
            PAST_KEY_ITEMS = new Dictionary<EPastKeyItem, PastKeyItem>();
            for (var key = EPastKeyItem.WindyWoodsKey; key < EPastKeyItem.MAX; key++)
            {
                PAST_KEY_ITEMS[key] = new PastKeyItem(key);
            }
        }

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterItemInRange(KEY_OFFSET, KEY_CEIL, id => new PastKeyItem(id));
        }

        public static bool AreAllKeysUnlocked(IUnlocksSource unlocks)
        {
            return PAST_KEY_ITEMS.Values.All(unlocks.IsUnlocked);
        }

        public static bool IsPastKeyReceived(IUnlocksSource unlocks, PastKeyKind key)
        {
            return unlocks.IsUnlocked(PAST_KEY_ITEMS[key]);
        }

        public static IEnumerable<PastKeyKind> GetPastKeysReceived(IUnlocksSource unlocks)
        {
            return PAST_KEY_ITEMS.Values.Where(unlocks.IsUnlocked).Select(item => item.Item).AsEnumerable();
        }

        private EPastKeyItem _key;

        public PastKeyItem(long id) : base(id)
        {
            _key = (EPastKeyItem)(id - KEY_OFFSET);
        }

        public PastKeyItem(EPastKeyItem key) : base((long)key + KEY_OFFSET)
        {
            _key = key;
        }

        public override EPastKeyItem Item => _key;
    }
}
