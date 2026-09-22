using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.services.client;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class StageDataPatchService
    {
        private MelonMod _melonMod;
        private IAPConnectionService _connectionService;

        private IDictionary<EWorldStage, EWorldStage> _stageUnlocks;

        private bool _missionRewardSkinsPatched = false;

        private bool _unlockCondsNeedPatching = false;

        public StageDataPatchService(MelonMod melonMod, IAPConnectionService connectionService)
        {
            _melonMod = melonMod;
            _connectionService = connectionService;

            _connectionService.OnConnect += _onConnect;
        }

        public void OnLateUpdate()
        {
            _initUnlockCondsIfNeeded();
            _patchMissionRewardSkins();
            _patchStageUnlockConds();
        }

        private void _initUnlockCondsIfNeeded()
        {
            if (_stageUnlocks == null && MasterData.StageList != null)
            {
                _initUnlockConds();
            }
        }

        private void _initUnlockConds()
        {
            _stageUnlocks = new Dictionary<EWorldStage, EWorldStage>();
            foreach (var stageInfo in MasterData.StageList.m_stageList)
            {
                EWorldStage stageId = (EWorldStage)stageInfo.stageId;
                if (stageId == EWorldStage.PacVillage)
                {
                    continue;
                }
                _stageUnlocks[stageId] = (EWorldStage)stageInfo.unlockCond[0];
            }
        }

        private void _onConnect()
        {
            _unlockCondsNeedPatching = true;
        }

        private void _patchStageUnlockConds()
        {
            if (!_unlockCondsNeedPatching || MasterData.StageList == null)
            {
                return;
            }
            _initUnlockCondsIfNeeded();
            _syncUnlockConds();
            _unlockCondsNeedPatching = false;
        }

        private void _syncUnlockConds()
        {
            foreach (var stageInfo in MasterData.StageList.m_stageList)
            {
                EWorldStage stageId = (EWorldStage)stageInfo.stageId;
                if (stageId == EWorldStage.PacVillage)
                {
                    continue;
                }
                EWorldStage cond = _getLevelUnlockCond(stageId);
                stageInfo.unlockCond.Clear();
                stageInfo.unlockCond.Add((int)cond);
            }
        }

        private EWorldStage _getLevelUnlockCond(EWorldStage stageId)
        {
            bool isLevelRando = _connectionService.IsLevelRando ?? true;
            if (stageId == EWorldStage.Stage7_1)
            {
                return isLevelRando ? EWorldStage.Stage6_5 : EWorldStage.Stage6_4;
            }
            return isLevelRando || _isBlockedStage(stageId) ? EWorldStage.Stage6_5 : _stageUnlocks[stageId];
        }

        private void _patchMissionRewardSkins()
        {
            if (_missionRewardSkinsPatched || (MasterData.MissionRewardList?.m_rewardList?.Count ?? 0) == 0)
            {
                return;
            }
            _missionRewardSkinsPatched = true;
            foreach (var missionRewardData in MasterData.MissionRewardList.m_rewardList)
            {
                if (missionRewardData.type != EMissionRewardType.Skin)
                {
                    continue;
                }
                missionRewardData.rewardId = 0;
            }
        }

        private static readonly HashSet<EWorldStage> m_blockedStages = new HashSet<EWorldStage>()
        {
            EWorldStage.Stage2_1,
            EWorldStage.Stage3_1,
            EWorldStage.Stage4_1,
            EWorldStage.Stage5_1,
            EWorldStage.Stage6_1,
            EWorldStage.Stage6_4,
            EWorldStage.Stage8_1,
            EWorldStage.Stage9_1,
            EWorldStage.Stage10_1,
            EWorldStage.Stage11_1,
            EWorldStage.Stage12_1,
            EWorldStage.Stage6_5
        };

        private bool _isBlockedStage(EWorldStage stageId)
        {
            return m_blockedStages.Contains(stageId);
        }
    }
}
