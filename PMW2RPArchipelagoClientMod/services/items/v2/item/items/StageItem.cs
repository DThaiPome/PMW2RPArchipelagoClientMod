using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item.items
{
    public class StageItem : APermanentUnlockableItem<EWorldStage>
    {
        private static readonly long LEVEL_OFFSET = 0;
        private static readonly long STAGE_ID_CEIL = LEVEL_OFFSET + (long)EWorldStage.StageSonic_1 + 1;

        static StageItem()
        {
            ServiceFactory.IdMapperService.RegisterItemInRange(LEVEL_OFFSET, STAGE_ID_CEIL, id => new StageItem(id));
        }

        private EWorldStage _stage;

        public StageItem(long id) : base(id)
        {
            _stage = (EWorldStage)(id - LEVEL_OFFSET - 1);
        }

        public StageItem(EWorldStage stage) : base((long)stage +  LEVEL_OFFSET + 1)
        {
            _stage = stage;
        }

        public override EWorldStage Item => _stage;
    }
}
