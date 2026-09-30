using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.COSTUME_OFFSET,
        exclusiveMax: WorldConstants.COSTUME_OFFSET + (long)EPlayerSkin.MAX)]
    public class SkinItem : APermanentUnlockableItem<EPlayerSkin>
    {
        public static Dictionary<EPlayerSkin, SkinItem> SKIN_ITEMS { get; private set; }

        static SkinItem()
        {
            SKIN_ITEMS = new Dictionary<EPlayerSkin, SkinItem>();
            for (var skin = EPlayerSkin.Hunter; skin < EPlayerSkin.MAX; skin++)
            {
                SKIN_ITEMS[skin] = new SkinItem(skin);
            }
        }

        public static bool IsSkinReceived(IUnlocksSource unlocks, EPlayerSkin skin)
        {
            return unlocks.IsUnlocked(SKIN_ITEMS[skin]);
        }

        EPlayerSkin _skin;

        public SkinItem(long id) : base(id)
        {
            _skin = (EPlayerSkin)(id - WorldConstants.COSTUME_OFFSET);
        }

        public SkinItem(EPlayerSkin skin) : base((long)skin + WorldConstants.COSTUME_OFFSET)
        {
            _skin = skin;
        }

        public override EPlayerSkin Item => _skin;
    }
}
