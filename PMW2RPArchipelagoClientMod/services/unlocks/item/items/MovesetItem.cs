using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;
using EMovesetItem = PMW2RPArchipelagoClientMod.models.data.MovesetKind;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.MOVEMENT_OFFSET,
        exclusiveMax: WorldConstants.MOVEMENT_OFFSET + (long)EMovesetItem.MAX)]
    public class MovesetItem : APermanentUnlockableItem<EMovesetItem>
    {
        public static Dictionary<EMovesetItem, MovesetItem> MOVESET_ITEMS { get; private set; }

        static MovesetItem()
        {
            MOVESET_ITEMS = new Dictionary<EMovesetItem, MovesetItem>();
            for (var moveset = EMovesetItem.ProgressiveButtBounce; moveset < EMovesetItem.MAX; moveset++)
            {
                MOVESET_ITEMS[moveset] = new MovesetItem(moveset);
            }
        }

        public static bool IsRevRollReceived(IUnlocksSource unlocks)
        {
            return unlocks.IsUnlocked(MOVESET_ITEMS[EMovesetItem.RevRoll]);
        }

        public static bool IsFlipKickReceived(IUnlocksSource unlocks)
        {
            return unlocks.IsUnlocked(MOVESET_ITEMS[EMovesetItem.FlipKick]);
        }

        public static bool IsDotThrowReceived(IUnlocksSource unlocks)
        {
            return unlocks.IsUnlocked(MOVESET_ITEMS[EMovesetItem.PacDotAttack]);
        }

        public static bool IsFlutterReceived(IUnlocksSource unlocks)
        {
            return unlocks.IsUnlocked(MOVESET_ITEMS[EMovesetItem.Flutter]);
        }

        public static ProgressiveButtBounce GetButtBounceLevel(IUnlocksSource unlocks)
        {
            return unlocks.GetCountReceived(MOVESET_ITEMS[EMovesetItem.ProgressiveButtBounce]) switch
            {
                0 => ProgressiveButtBounce.None,
                1 => ProgressiveButtBounce.ButtBounce,
                _ => ProgressiveButtBounce.SuperButtBounce
            };
        }

        public static ProgressiveDolphinKick GetDolphinKickLevel(IUnlocksSource unlocks)
        {
            return unlocks.GetCountReceived(MOVESET_ITEMS[EMovesetItem.ProgressiveDolphinKick]) switch
            {
                0 => ProgressiveDolphinKick.None,
                1 => ProgressiveDolphinKick.DolphinKick,
                _ => ProgressiveDolphinKick.SuperDolphinKick
            };
        }

        private EMovesetItem _moveset;

        public MovesetItem(long id) : base(id)
        {
            _moveset = (EMovesetItem)(id - WorldConstants.MOVEMENT_OFFSET);
        }

        public MovesetItem(EMovesetItem moveset) : base((long)moveset + WorldConstants.MOVEMENT_OFFSET)
        {
            _moveset = moveset;
        }

        public override EMovesetItem Item => _moveset;
    }
}
