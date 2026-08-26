using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
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
            if (id >= GOLDEN_FRUIT_OFFSET && id < KEY_OFFSET)
            {
                return _mapGoldenFruitItem(id);
            }
            if (id >= KEY_OFFSET && id < COSTUME_OFFSET)
            {
                return _mapPastKeyItem(id); 
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

        private IItemMapEntry _mapGoldenFruitItem(long id)
        {
            GoldenFruitItem item = (GoldenFruitItem)(id - GOLDEN_FRUIT_OFFSET);
            if (item >= GoldenFruitItem.MAX)
            {
                return new UnknownItemResult(id);
            }
            return new GoldenFruitItemResult(item);
        }

        private IItemMapEntry _mapPastKeyItem(long id)
        {
            PastKeyItem item = (PastKeyItem)(id - KEY_OFFSET);
            if (item >= PastKeyItem.MAX)
            {
                return new UnknownItemResult(id);
            }
            return new PastKeyItemResult(item);
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
            if (id >= MISSION_OFFSET && id < GASHAPON_OFFSET)
            {
                return _mapMission(id);
            }
            if (id >= GASHAPON_OFFSET && id < GALAXIAN_OFFSET)
            {
                return _mapCapsule(id);
            }
            if (id >= GALAXIAN_OFFSET && id < CHERRY_OFFSET)
            {
                return _mapGalaxian(id);
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

        private ILocationMapEntry _mapMission(long id)
        {
            EMissionKind kind = (EMissionKind)(id - MISSION_OFFSET);
            if (kind <= EMissionKind.None || kind >= EMissionKind.Mission100)
            {
                return new UnknownLocationResult(id);
            }
            return new MissionLocationResult(kind);
        }

        private ILocationMapEntry _mapGalaxian(long id)
        {
            int mazeId = (int)(id - GALAXIAN_OFFSET);
            if (mazeId >= 15)
            {
                return new UnknownLocationResult(id);
            }
            return new UnlockMazeLocationResult(mazeId);
        }

        private ILocationMapEntry _mapCapsule(long id)
        {
            ECapsule capsule = (ECapsule)(id - GASHAPON_OFFSET);
            if (capsule <= ECapsule.None || capsule > ECapsule.Capsule54)
            {
                return new UnknownLocationResult(id);
            }
            return new CollectCapsuleLocationResult(capsule);
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

        public long MissionToClearMissionLocationId(EMissionKind kind)
        {
            return (long)kind + MISSION_OFFSET;
        }

        public long MazeUnlockToUnlockMazeLocationId(int mazeId)
        {
            return mazeId + GALAXIAN_OFFSET;
        }

        public long CapsuleToCollectCapsuleLocationId(ECapsule capsule)
        {
            return (long)capsule + GASHAPON_OFFSET;
        }
    }
}
