using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class LevelUnlockSyncService
    {
        private MelonMod _melonMod;
        private IUnlocksSource _unlocks;


        private static List<EScene> _outOfGameScenes = new List<EScene>()
        {
            EScene.None,
            EScene.Title,
            EScene.Logo,
            EScene.Movie,
            EScene.Original,
            EScene.Credit,
            EScene.CreditNew,
            EScene.CreditSonic,
            EScene.LoginXbox,
            EScene.LaunchActivity,
            EScene.GhostIsland_Title,
            EScene.Scene_MazeStageCredit,
            EScene.FirstScene,
            EScene.Dummy
        };
        private static bool _isOutOfGame
        {
            get
            {
                var sceneManagerInstance = SceneManager.Instance;
                if (sceneManagerInstance == null)
                {
                    return true;
                }
                return _outOfGameScenes.Contains(SceneManager.Instance.m_eCurrentScene);
            }
        }

        public LevelUnlockSyncService(MelonMod melonMod,
            IUnlocksSource unlocks)
        {
            _melonMod = melonMod;
            _unlocks = unlocks;
        }

        public void OnLateUpdate()
        {
            if (_isOutOfGame)
            {
                return;
            }
            _syncLevelUnlocks();
            _syncAreaUnlocks();
            _syncPastUnlocked();
        }

        private void _syncLevelUnlocks()
        {
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                bool unlocked = _unlocks.Stages.GetValueOrDefault(stage, false);
                EStageFlag stageFlag = PACWSaveData.GetStageFlag((int)stage);
                if (!unlocked && stageFlag != EStageFlag.Locked)
                {
                    _melonMod.LoggerInstance.Msg("LOCKING STAGE: " + stage.ToString());
                    PACWSaveData.SetStageFlag((int)stage, EStageFlag.Locked, force: true);
                }
                else if (unlocked && stageFlag == EStageFlag.Locked)
                {
                    _melonMod.LoggerInstance.Msg("UNLOCKING STAGE: " + stage.ToString());
                    PACWSaveData.SetStageFlag((int)stage, EStageFlag.Unlock, force: true);
                }
            }
        }

        private void _syncAreaUnlocks()
        {
            _syncAreaUnlock(EUnlockSSKind.Area1, EWorldStage.Stage1_1, EWorldStage.Stage1_2, EWorldStage.Stage1_3, EWorldStage.Stage1_4);
            _syncAreaUnlock(EUnlockSSKind.Area2, EWorldStage.Stage2_1, EWorldStage.Stage2_2, EWorldStage.Stage2_3, EWorldStage.Stage2_4);
            _syncAreaUnlock(EUnlockSSKind.Area3, EWorldStage.Stage3_1, EWorldStage.Stage3_2, EWorldStage.Stage3_3, EWorldStage.Stage3_4);
            _syncAreaUnlock(EUnlockSSKind.Area4, EWorldStage.Stage4_1, EWorldStage.Stage4_2, EWorldStage.Stage4_3, EWorldStage.Stage4_4);
            _syncAreaUnlock(EUnlockSSKind.Area5, EWorldStage.Stage5_1, EWorldStage.Stage5_2, EWorldStage.Stage5_3, EWorldStage.Stage5_4);
            _syncAreaUnlock(EUnlockSSKind.Area6, EWorldStage.Stage6_1, EWorldStage.Stage6_2, EWorldStage.Stage6_3);
            _syncAreaUnlock(EUnlockSSKind.DotRail, EWorldStage.Stage6_4);
            _syncAreaUnlock(EUnlockSSKind.Area7, EWorldStage.Stage7_1, EWorldStage.Stage7_2);
            _syncAreaUnlock(EUnlockSSKind.Area8, EWorldStage.Stage8_1, EWorldStage.Stage8_2);
            _syncAreaUnlock(EUnlockSSKind.Area9, EWorldStage.Stage9_1, EWorldStage.Stage9_2);
            _syncAreaUnlock(EUnlockSSKind.Area10, EWorldStage.Stage10_1, EWorldStage.Stage10_2);
            _syncAreaUnlock(EUnlockSSKind.Area11, EWorldStage.Stage11_1, EWorldStage.Stage11_2);
            _syncAreaUnlock(EUnlockSSKind.Area12, EWorldStage.Stage12_1);
            _syncAreaUnlock(EUnlockSSKind.DotRail_Past, EWorldStage.Stage12_2);
            _syncAreaUnlock(EUnlockSSKind.Tocman, EWorldStage.Stage6_5);
        }

        private void _syncAreaUnlock(EUnlockSSKind area, params EWorldStage[] stages)
        {
            if (PACWSaveData.IsUnlockStageSelect(area))
            {
                return;
            }

            if (stages.AsEnumerable().Any(stage => PACWSaveData.GetStageFlag((int)stage) != EStageFlag.Locked))
            {
                PACWSaveData.SetUnlockStageSelect(area, true);
            }
        }

        private static List<EWorldStage> _pastStages = new List<EWorldStage>()
        {
            EWorldStage.Stage7_1,
            EWorldStage.Stage7_2,
            EWorldStage.Stage8_1,
            EWorldStage.Stage8_2,
            EWorldStage.Stage9_1,
            EWorldStage.Stage9_2,
            EWorldStage.Stage10_1,
            EWorldStage.Stage10_2,
            EWorldStage.Stage11_1,
            EWorldStage.Stage11_2,
            EWorldStage.Stage12_1,
            EWorldStage.Stage12_2,
        };

        private void _syncPastUnlocked()
        {
            if (!PACWSaveData.IsEnterPast() && _pastStages.Any(stage => _unlocks.Stages.GetValueOrDefault(stage, false)))
            {
                PACWSaveData.SetEnterPast(true);
            }
        }
    }
}
