using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    [MapFromIdRange(
        inclusiveMin: WorldConstants.LEVEL_OFFSET,
        exclusiveMax: WorldConstants.LEVEL_OFFSET + (long)EWorldStage.StageSonic_1 + 1)]
    public class StageItem : APermanentUnlockableItem<EWorldStage>
    {
        public static Dictionary<EWorldStage, StageItem> STAGE_ITEMS { get; private set; }

        static StageItem()
        {
            STAGE_ITEMS = new Dictionary<EWorldStage, StageItem>();
            for (var stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                STAGE_ITEMS[stage] = new StageItem(stage);
            }
        }

        public static bool IsStageReceived(IUnlocksSource unlocks, EWorldStage stage)
        {
            return unlocks.IsUnlocked(STAGE_ITEMS[stage]);
        }

        private EWorldStage _stage;

        public StageItem(long id) : base(id)
        {
            _stage = (EWorldStage)(id - WorldConstants.LEVEL_OFFSET - 1);
        }

        public StageItem(EWorldStage stage) : base((long)stage + WorldConstants.LEVEL_OFFSET + 1)
        {
            _stage = stage;
        }

        public override EWorldStage Item => _stage;
    }
}
