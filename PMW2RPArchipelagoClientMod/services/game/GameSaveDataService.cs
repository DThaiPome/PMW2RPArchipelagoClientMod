using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class GameSaveDataService : IGameSaveDataService
    {
        private MelonMod _melonMod;
        private ActiveSceneService _activeSceneService;

        public bool SaveOperationsAllowed => _activeSceneService.InLoadedSave;

        public GameSaveDataService(MelonMod melonMod,
            ActiveSceneService active)
        {
            _melonMod = melonMod;
            _activeSceneService = active;
        }

        private void _assertOpAllowed()
        {
            if (!SaveOperationsAllowed)
            {
                throw new InvalidDataException("Cannot do save operations");
            }
        }

        public EStageFlag GetStageFlag(EWorldStage stage)
        {
            _assertOpAllowed();
            return PACWSaveData.GetStageFlag((int)stage);
        }

        public void SetStageFlag(EWorldStage stage, EStageFlag flag)
        {
            _assertOpAllowed();
            PACWSaveData.SetStageFlag((int)stage, flag, force: true);
        }

        public bool IsUnlockStageSelect(EUnlockSSKind area)
        {
            _assertOpAllowed();
            return PACWSaveData.IsUnlockStageSelect(area);
        }

        public void SetUnlockStageSelect(EUnlockSSKind area, bool unlocked)
        {
            _assertOpAllowed();
            PACWSaveData.SetUnlockStageSelect(area, unlocked);
        }

        public bool IsEnterPast()
        {
            _assertOpAllowed();
            return PACWSaveData.IsEnterPast();
        }

        public void SetEnterPast(bool enterPast)
        {
            _assertOpAllowed();
            PACWSaveData.SetEnterPast(enterPast);
        }

        public EMissionFlag GetMissionFlag(EMissionKind kind)
        {
            _assertOpAllowed();
            return PACWSaveData.GetMissionFlag(kind);
        }

        public void SetMissionFlag(EMissionKind kind, EMissionFlag flag)
        {
            _assertOpAllowed();
            PACWSaveData.SetMissionFlag(kind, flag);
        }
    }
}
