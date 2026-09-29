using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.item;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item.items
{
    public class StageItem : APermanentUnlockableItem<EWorldStage>
    {
        private static readonly long LEVEL_OFFSET = 0;
        private static readonly long STAGE_ID_CEIL = LEVEL_OFFSET + (long)EWorldStage.StageSonic_1 + 1;

        public static Dictionary<EWorldStage, StageItem> STAGE_ITEMS { get; private set; }

        static StageItem()
        {
            STAGE_ITEMS = new Dictionary<EWorldStage, StageItem>();
            for (var stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                STAGE_ITEMS[stage] = new StageItem(stage);
            }
        }

        public static void Register()
        {
            ServiceFactory.IdMapperService.RegisterItemInRange(LEVEL_OFFSET, STAGE_ID_CEIL, id => new StageItem(id));
        }

        public static bool IsStageReceived(IUnlocksSource unlocks, EWorldStage stage)
        {
            return unlocks.IsUnlocked(STAGE_ITEMS[stage]);
        }

        private EWorldStage _stage;

        public StageItem(long id) : base(id)
        {
            _stage = (EWorldStage)(id - LEVEL_OFFSET - 1);
        }

        public StageItem(EWorldStage stage) : base((long)stage + LEVEL_OFFSET + 1)
        {
            _stage = stage;
        }

        public override EWorldStage Item => _stage;
    }
}
