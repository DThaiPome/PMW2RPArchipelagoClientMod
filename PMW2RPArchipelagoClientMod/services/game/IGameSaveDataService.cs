using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public interface IGameSaveDataService
    {
        bool SaveOperationsAllowed { get; }
        EStageFlag GetStageFlag(EWorldStage stage);
        void SetStageFlag(EWorldStage stage, EStageFlag flag);
        bool IsUnlockStageSelect(EUnlockSSKind area);
        void SetUnlockStageSelect(EUnlockSSKind area, bool unlocked);
        bool IsEnterPast();
        void SetEnterPast(bool enterPast);
        EMissionFlag GetMissionFlag(EMissionKind kind);
        void SetMissionFlag(EMissionKind kind, EMissionFlag flag);
    }
}
