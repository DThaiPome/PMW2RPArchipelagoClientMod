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
        bool IsSpookyUnlockedOrPlayed()
        {
            var stageFlag = GetStageFlag(EWorldStage.Stage6_4);
            return stageFlag == EStageFlag.Unlock || stageFlag == EStageFlag.Played;
        }
        bool IsUnlockStageSelect(EUnlockSSKind area);
        void SetUnlockStageSelect(EUnlockSSKind area, bool unlocked);
        bool IsEnterPast();
        void SetEnterPast(bool enterPast);
        EMissionFlag GetMissionFlag(EMissionKind kind);
        void SetMissionFlag(EMissionKind kind, EMissionFlag flag);
        bool CheckMazeUnlock(int mazeId);
        void UnlockMaze(int mazeId);
    }
}
