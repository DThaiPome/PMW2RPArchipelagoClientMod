using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items;
using UnityEngine;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class LevelUnlockSyncService
    {
        private MelonMod _melonMod;
        private IUnlocksSource _unlocks;
        private ILocationsSource _locations;
        private IGameSaveDataService _gameSaveDataService;
        private IAPConnectionService _apConnectionService;
        private StageSelectCinematicService _stageSelectCinematicService;
        private ActiveSceneService _activeSceneService;
        private PlayerPacmanStateService _playerPacmanStateService;

        public LevelUnlockSyncService(MelonMod melonMod,
            IUnlocksSource unlocks,
            ILocationsSource locations,
            IGameSaveDataService gameSaveDataService,
            IAPConnectionService apConnectionService,
            StageSelectCinematicService stageSelectCinematicService,
            ActiveSceneService activeSceneService,
            PlayerPacmanStateService playerPacmanStateService)
        {
            _melonMod = melonMod;
            _unlocks = unlocks;
            _locations = locations;
            _gameSaveDataService = gameSaveDataService;
            _apConnectionService = apConnectionService;
            _stageSelectCinematicService = stageSelectCinematicService;
            _activeSceneService = activeSceneService;
            _playerPacmanStateService = playerPacmanStateService;
        }

        public void OnLateUpdate()
        {
            if (!_gameSaveDataService.SaveOperationsAllowed)
            {
                return;
            }
            _syncLevelUnlocks();
            _syncAreaUnlocks();
            _syncPastUnlocked();
            _syncStagesCleared();
            _syncMissionsCleared();
            _syncMazesUnlocked();
            _syncFruitLevelUnlocks();
            _syncGoldMedalsCleared();
            _syncSkinUnlocks();
            _flushFillerUnlocks();
        }

        private void _syncLevelUnlocks()
        {
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                bool unlocked = _unlocks.Stages.GetValueOrDefault(stage, false);
                EStageFlag stageFlag = _gameSaveDataService.GetStageFlag(stage);
                if (unlocked && stageFlag == EStageFlag.Locked)
                {
                    if (stage == EWorldStage.Stage6_5 && !_unlocks.AreAllKeysUnlocked())
                    {
                        continue;
                    }
                    _unlockStage(stage);
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
            if (_gameSaveDataService.IsUnlockStageSelect(area))
            {
                return;
            }

            if (stages.AsEnumerable().Any(stage => _gameSaveDataService.GetStageFlag(stage) != EStageFlag.Locked))
            {
                _gameSaveDataService.SetUnlockStageSelect(area, true);
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
            if (!_gameSaveDataService.IsEnterPast() && _pastStages.Any(stage => _unlocks.Stages.GetValueOrDefault(stage, false)))
            {
                _gameSaveDataService.SetEnterPast(true);
            }
        }

        private void _syncStagesCleared()
        {
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                EStageFlag flag = _gameSaveDataService.GetStageFlag(stage);

                if (flag == EStageFlag.Clear && !_locations.ClearedStages.Contains(stage))
                {
                    _locations.ClearStage(stage);
                }
                else if (flag != EStageFlag.Clear && _locations.ClearedStages.Contains(stage))
                {
                    _gameSaveDataService.SetStageFlag(stage, EStageFlag.Clear);
                }
            }
        }
        
        private void _syncMissionsCleared()
        {
            for (EMissionKind kind = EMissionKind.Mission1; kind < EMissionKind.Mission99; kind++)
            {
                EMissionFlag flag = _gameSaveDataService.GetMissionFlag(kind);
                
                if (flag == EMissionFlag.Achieved && !_locations.ClearedMissions.Contains(kind))
                {
                    _locations.ClearMission(kind);
                }
                else if (flag != EMissionFlag.Achieved && _locations.ClearedMissions.Contains(kind))
                {
                    _gameSaveDataService.SetMissionFlag(kind, EMissionFlag.Achieved);
                }
            }
        }

        private void _syncMazesUnlocked()
        {
            for (int mazeId = 0; mazeId < 15; mazeId++)
            {
                bool unlocked = _gameSaveDataService.CheckMazeUnlock(mazeId);
                if (unlocked && !_locations.UnlockedMazes.Contains(mazeId))
                {
                    _locations.UnlockMaze(mazeId);
                }
                else if (!unlocked && _locations.UnlockedMazes.Contains(mazeId))
                {
                    // TODO: This might not do anything if a maze gets unlocked remotely while that level is actually being played. Find a way to fix this maybe, not urgent.
                    _gameSaveDataService.UnlockMaze(mazeId);
                }
            }
        }

        private void _syncFruitLevelUnlocks()
        {
            if (_apConnectionService.IsLevelRando ?? true)
            {
                return;
            }
            foreach (var goldenFruitItem in _unlocks.GoldenFruit)
            {
                var stageId = _goldenFruitToLevelUnlock(goldenFruitItem);
                if (_gameSaveDataService.GetStageFlag(stageId) == EStageFlag.Locked)
                {
                    _melonMod.LoggerInstance.Msg("UNLOCKING STAGE FROM GOLDEN FRUIT: " + stageId);
                    _unlockStage(stageId);
                }
            }
            if (_unlocks.AreAllGoldenFruitsUnlocked() && _gameSaveDataService.GetStageFlag(EWorldStage.Stage6_4) == EStageFlag.Locked)
            {
                _melonMod.LoggerInstance.Msg("UNLOCKING SPOOKY FROM FRUITS");
                _gameSaveDataService.SetStageFlag(EWorldStage.Stage6_4, EStageFlag.Unlock);
            }
            if (_unlocks.AreAllKeysUnlocked() && _gameSaveDataService.GetStageFlag(EWorldStage.Stage6_5) == EStageFlag.Locked)
            {
                _melonMod.LoggerInstance.Msg("UNLOCKING TOC-MAN FROM KEYS");
                _unlockStage(EWorldStage.Stage6_5);
            }
        }

        private EWorldStage _goldenFruitToLevelUnlock(GoldenFruitItem goldenFruitItem)
        {
            return goldenFruitItem switch
            {
                GoldenFruitItem.GoldenCherry => EWorldStage.Stage2_1,
                GoldenFruitItem.GoldenStrawberry => EWorldStage.Stage3_1,
                GoldenFruitItem.GoldenApple => EWorldStage.Stage4_1,
                GoldenFruitItem.GoldenOrange => EWorldStage.Stage5_1,
                GoldenFruitItem.GoldenMelon => EWorldStage.Stage6_1,
                _ => throw new NotImplementedException("what kinda golden fruit is this")
            };
        }

        private void _unlockStage(EWorldStage stage)
        {
            if (stage == EWorldStage.Stage6_4)
            {
                _gameSaveDataService.SetStageFlag(stage, EStageFlag.Unlock);
                return;
            }

            if (_stageSelectCinematicService.IsStageQueued(stage))
            {
                return;
            }
            if (_activeSceneService.OnStageSelect || !_stageSelectCinematicService.EnqueueUnlock(stage))
            {
                _gameSaveDataService.SetStageFlag(stage, EStageFlag.Unlock);
            }
        }

        private void _syncGoldMedalsCleared()
        {
            foreach (var stageInfo in MasterData.StageList.m_stageList)
            {
                EWorldStage stageId = (EWorldStage)stageInfo.stageId;
                if (stageInfo.stageId == (int)EWorldStage.Stage5_3)
                {
                    continue;
                }
                double time = PACWSaveData.GetStageTime((int)stageId);
                EEstimateTime medal = time == 0 ? EEstimateTime.None : stageInfo.GetMedalKind(time + 0.01);
                if (medal == EEstimateTime.Gold && !_locations.ClearedGoldMedals.Contains(stageId))
                {
                    _locations.ClearGoldMedal(stageId);
                }
                else if (medal != EEstimateTime.Gold && _locations.ClearedGoldMedals.Contains(stageId))
                {
                    PACWSaveData.SetStageTime((int)stageId, (stageInfo.estimateTimeG - 1) / 100.0);
                }
            }
        }

        private static HashSet<EPlayerSkin> _ignoredSkins = new HashSet<EPlayerSkin>()
        {
            EPlayerSkin.Street,
            EPlayerSkin.Street2,
            EPlayerSkin.Street3,
            EPlayerSkin.BirthDay,
            EPlayerSkin.Xmas,
            EPlayerSkin.Xmas2,
            EPlayerSkin.Xmas3,
            EPlayerSkin.Sonic
        };

        private void _syncSkinUnlocks()
        {
            for (EPlayerSkin skin = EPlayerSkin.Hunter; skin < EPlayerSkin.MAX; skin++)
            {
                if (_ignoredSkins.Contains(skin))
                {
                    continue;
                }
                bool unlockedInSave = _gameSaveDataService.IsSkinUnlocked(skin);
                bool unlockedInWorld = _unlocks.Skins.Contains(skin);
                if (!unlockedInSave && unlockedInWorld)
                {
                    _gameSaveDataService.SetSkinUnlocked(skin, true);
                }
                else if (unlockedInSave && !unlockedInWorld)
                {
                    _gameSaveDataService.SetSkinUnlocked(skin, false);
                    if (_gameSaveDataService.GetPlayerSkin() == skin)
                    {
                        _gameSaveDataService.SetPlayerSkin(EPlayerSkin.Normal);
                    }
                }
            }
        }

        private void _flushFillerUnlocks()
        {
            if (!_activeSceneService.InNonVillageStage || !_playerPacmanStateService.IsInMoveState)
            {
                return;
            }

            int dots = _unlocks.FlushPacDots();
            if (dots > 0)
            {
                StageStateManager.AddPacDot(dots);
            }

            int score = _unlocks.FlushPoints();
            if (score > 0)
            {
                StageStateManager.AddScore(EStageScore.Dot, Vector3.zero, score);
            }
        }
    }
}
