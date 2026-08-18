using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.mapping.items;
using PMW2RPArchipelagoClientMod.services.items.mapping.locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping
{
    public class CheckIdMapperService : ICheckIdMapperService
    {
        private static readonly long LEVEL_OFFSET = 0L;
        private static readonly long GOLDEN_FRUIT_OFFSET = 100L;
        private static readonly long KEY_OFFSET = 200L;
        private static readonly long COSTUME_OFFSET = 300L;
        private static readonly long FRUIT_SWITCH_OFFSET = 400L;
        private static readonly long MOVEMENT_OFFSET = 500L;
        private static readonly long TIMETRIAL_OFFSET = 1000L;
        private static readonly long MISSION_OFFSET = 2000L;
        private static readonly long GASHAPON_OFFSET = 3000L;
        private static readonly long GALAXIAN_OFFSET = 4000L;
        private static readonly long CHERRY_OFFSET = 5000L;
        private static readonly long STRAWBERRY_OFFSET = 6000L;
        private static readonly long ORANGE_OFFSET = 7000L;
        private static readonly long APPLE_OFFSET = 8000L;
        private static readonly long MELON_OFFSET = 9000L;
        private static readonly long FILLER_OFFSET = 10000L;
        private static readonly long TRAP_OFFSET = 11000L;


        public CheckIdMapperService() { }

        public IItemMapEntry MapItem(ItemInfo itemInfo)
        {
            long id = itemInfo.ItemId;
            if (id >= LEVEL_OFFSET && id < GOLDEN_FRUIT_OFFSET)
            {
                return _mapStageItem(id);
            }
            if (id >= MOVEMENT_OFFSET && id < TIMETRIAL_OFFSET)
            {
                return _mapMoveset(id);
            }
            return new UnknownItemResult(id);
        }

        private IItemMapEntry _mapStageItem(long id)
        {
            if (!_dataIdToStage(id, out EWorldStage stage))
            {
                return new UnknownItemResult(id);
            }
            return new StageItemResult(stage);
        }

        private IItemMapEntry _mapMoveset(long id)
        {
            return (MovesetItem)(id - MOVEMENT_OFFSET) switch
            {
                MovesetItem.ProgressiveButtBounce => new ButtBounceItemResult(),
                MovesetItem.FlipKick => new FlipKickItemResult(),
                MovesetItem.RevRoll => new DashItemResult(),
                MovesetItem.PacDotAttack => new BombItemResult(),
                MovesetItem.ProgressiveDolphinKick => new DolphinKickItemResult(),
                MovesetItem.Flutter => new FlutterItemResult(),
                _ => new UnknownItemResult(id)
            };
        }

        public ILocationMapEntry MapLocation(long id)
        {
            if (id >= LEVEL_OFFSET && id < GOLDEN_FRUIT_OFFSET)
            {
                return _mapStageLocation(id);
            }
            return new UnknownLocationResult(id);
        }

        private ILocationMapEntry _mapStageLocation(long id)
        {
            if (!_dataIdToStage(id, out EWorldStage stage))
            {
                return new UnknownLocationResult(id);
            }
            return new StageLocationResult(stage);
        }

        private bool _dataIdToStage(long id, out EWorldStage stageId)
        {
            stageId = (EWorldStage)(id - 1);
            return stageId < EWorldStage.MAX;
        }

        public long StageToClearStageLocationId(EWorldStage stage)
        {
            return (long)stage + 1;
        }
    }
}
