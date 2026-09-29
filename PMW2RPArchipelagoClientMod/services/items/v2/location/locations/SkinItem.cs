using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.location.locations
{
    public class SkinItem : APermanentUnlockableItem<EPlayerSkin>
    {
        private static readonly long COSTUME_OFFSET = 300;
        private static readonly long COSTUME_CEIL = COSTUME_OFFSET + (long)EPlayerSkin.MAX;

        static SkinItem()
        {
            ServiceFactory.IdMapperService.RegisterItemInRange(COSTUME_OFFSET, COSTUME_CEIL, id => new SkinItem(id));
        }

        EPlayerSkin _skin;

        public SkinItem(long id) : base(id)
        {
            _skin = (EPlayerSkin)(id - COSTUME_OFFSET);
        }

        public SkinItem(EPlayerSkin skin) : base((long)skin + COSTUME_OFFSET)
        {
            _skin = skin;
        }

        public override EPlayerSkin Item => _skin;
    }
}
