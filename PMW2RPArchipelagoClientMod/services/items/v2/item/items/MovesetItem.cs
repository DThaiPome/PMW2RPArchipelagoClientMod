using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMovesetItem = PMW2RPArchipelagoClientMod.models.data.MovesetItem;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.items
{
    public class MovesetItem : APermanentUnlockableItem<EMovesetItem>
    {
        private static readonly long MOVEMENT_OFFSET = 500;
        private static readonly long MOVEMENT_CEIL = MOVEMENT_OFFSET + (long)EMovesetItem.MAX;

        static MovesetItem()
        {
            ServiceFactory.IdMapperService.RegisterItemInRange(MOVEMENT_OFFSET, MOVEMENT_CEIL, id => new MovesetItem(id));
        }

        private EMovesetItem _moveset;

        public MovesetItem(long id) : base(id)
        {
            _moveset = (EMovesetItem)(id -  MOVEMENT_OFFSET);
        }

        public MovesetItem(EMovesetItem moveset) : base((long)moveset + MOVEMENT_OFFSET)
        {
            _moveset = moveset;
        }

        public override EMovesetItem Item => _moveset;
    }
}
