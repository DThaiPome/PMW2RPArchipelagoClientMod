using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPastKeyItem = PMW2RPArchipelagoClientMod.models.data.PastKeyItem;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.items
{
    public class PastKeyItem : APermanentUnlockableItem<EPastKeyItem>
    {
        private static readonly long KEY_OFFSET = 200;
        private static readonly long KEY_CEIL = KEY_OFFSET + (long)EPastKeyItem.MAX;

        static PastKeyItem()
        {
            ServiceFactory.IdMapperService.RegisterItemInRange(KEY_OFFSET, KEY_CEIL, id => new PastKeyItem(id));
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
