using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.mapping
{
    public interface ICheckIdMapperService
    {
        IItemMapEntry MapItem(ItemInfo itemInfo);
        long StageToClearStageLocationId(EWorldStage stage);
        long MissionToClearMissionLocationId(EMissionKind kind);
        ILocationMapEntry MapLocation(long id);
    }
}
