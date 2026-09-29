using Il2Cpp;
using Il2CppUI;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items;
using UnityEngine;
using PMW2RPArchipelagoClientMod.services.items.item.items;
using PMW2RPArchipelagoClientMod.services.items.location.locations;
using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item;
using PMW2RPArchipelagoClientMod.services.items.location;

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

        private int _pendingPacDots;
        private int _pendingLives;
        private int _pendingPoints;

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

            var consumablesDispatcher = new ConsumableDelegates();
            consumablesDispatcher.OnGivePacDots += _onReceivePacDots;
            consumablesDispatcher.OnGiveLives += _onReceiveLives;
            consumablesDispatcher.OnGivePoints += _onReceivePoints;
            _unlocks.GiveConsumableReceiver(consumablesDispatcher);
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
            _syncWorldKeyLevelUnlocks();
            _syncGoldMedalsCleared();
            _syncSkinUnlocks();
            _flushFillerUnlocks();
        }

        private void _syncLevelUnlocks()
        {
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                bool unlocked = StageItem.IsStageReceived(_unlocks, stage);
                EStageFlag stageFlag = _gameSaveDataService.GetStageFlag(stage);
                if (unlocked && stageFlag == EStageFlag.Locked)
                {
                    if (stage == EWorldStage.Stage6_5)
                    {
                        if (PastKeyItem.AreAllKeysUnlocked(_unlocks)
                            && _gameSaveDataService.GetStageFlag(EWorldStage.Stage6_4) == EStageFlag.Clear)
                        {
                            _unlockStage(stage);
                        }
                    }
                    else
                    {
                        _unlockStage(stage);
                    }
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
            if (!_gameSaveDataService.IsEnterPast() && _pastStages.Any(stage => _gameSaveDataService.GetStageFlag(stage) != EStageFlag.Locked))
            {
                _gameSaveDataService.SetEnterPast(true);
            }
        }

        private void _syncStagesCleared()
        {
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                EStageFlag flag = _gameSaveDataService.GetStageFlag(stage);
                bool stageClearRemotely = StageClearLocation.IsStageClear(_locations, stage);

                if (flag == EStageFlag.Clear && !stageClearRemotely)
                {
                    StageClearLocation.ClearStage(_locations, stage);
                }
                else if (flag != EStageFlag.Locked && flag != EStageFlag.Clear && stageClearRemotely)
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
                bool missionClearRemotely = MissionClearLocation.IsMissionClear(_locations, kind);
                
                if (flag == EMissionFlag.Achieved && !missionClearRemotely)
                {
                    MissionClearLocation.ClearMission(_locations, kind);
                }
                else if (flag != EMissionFlag.Achieved && missionClearRemotely)
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
                bool unlockedRemotely = GalaxianCollectedLocation.IsGalaxianCollected(_locations, mazeId);
                if (unlocked && !unlockedRemotely)
                {
                    GalaxianCollectedLocation.ClearGalaxianCollected(_locations, mazeId);
                }
                else if (!unlocked && unlockedRemotely)
                {
                    // TODO: This might not do anything if a maze gets unlocked remotely while that level is actually being played. Find a way to fix this maybe, not urgent.
                    _gameSaveDataService.UnlockMaze(mazeId);
                }
            }
        }

        private void _syncWorldKeyLevelUnlocks()
        {
            if (_apConnectionService.IsLevelRando ?? true)
            {
                return;
            }

            foreach (var goldenFruitItem in GoldenFruitItem.GetReceivedGoldenFruits(_unlocks))
            {
                var stageId = _goldenFruitToLevelUnlock(goldenFruitItem);
                if (_gameSaveDataService.GetStageFlag(stageId) == EStageFlag.Locked)
                {
                    _unlockStage(stageId);
                }
            }
            if (GoldenFruitItem.AreAllGoldenFruitsUnlocked(_unlocks) && _gameSaveDataService.GetStageFlag(EWorldStage.Stage6_4) == EStageFlag.Locked)
            {
                _gameSaveDataService.SetStageFlag(EWorldStage.Stage6_4, EStageFlag.Unlock);
            }

            foreach (var pastKeyItem in PastKeyItem.GetPastKeysReceived(_unlocks))
            {
                var stageId = _keyToLevelUnlock(pastKeyItem);
                if (_gameSaveDataService.GetStageFlag(stageId) == EStageFlag.Locked)
                {
                    _unlockStage(stageId);
                }
            }
            if (PastKeyItem.AreAllKeysUnlocked(_unlocks)
                && _gameSaveDataService.GetStageFlag(EWorldStage.Stage6_5) == EStageFlag.Locked
                && _gameSaveDataService.GetStageFlag(EWorldStage.Stage6_4) == EStageFlag.Clear)
            {
                _unlockStage(EWorldStage.Stage6_5);
            }
        }

        private EWorldStage _goldenFruitToLevelUnlock(EFruits goldenFruitItem)
        {
            return goldenFruitItem switch
            {
                EFruits.Cherry => EWorldStage.Stage2_1,
                EFruits.Strawberry => EWorldStage.Stage3_1,
                EFruits.Apple => EWorldStage.Stage4_1,
                EFruits.Orange => EWorldStage.Stage5_1,
                EFruits.Melon => EWorldStage.Stage6_1,
                _ => throw new NotImplementedException("what kinda golden fruit is this")
            };
        }

        private EWorldStage _keyToLevelUnlock(PastKeyKind pastKeyItem)
        {
            return pastKeyItem switch
            {
                PastKeyKind.WindyWoodsKey => EWorldStage.Stage8_1,
                PastKeyKind.ThunderSnowMountainKey => EWorldStage.Stage9_1,
                PastKeyKind.FieryCavernsKey => EWorldStage.Stage10_1,
                PastKeyKind.DimUnderwatersKey => EWorldStage.Stage11_1,
                PastKeyKind.GhostIslandKey => EWorldStage.Stage12_1,
                _ => throw new NotImplementedException("what kinda key is this")
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
                if (stageId == EWorldStage.PacVillage || stageId == EWorldStage.Stage5_3 || stageId >= EWorldStage.StageSonic_1)
                {
                    continue;
                }
                double time = PACWSaveData.GetStageTime((int)stageId);
                EEstimateTime medal = time == 0 ? EEstimateTime.None : stageInfo.GetMedalKind(time + 0.01);
                bool goldMedalClearedRemotely = GoldMedalClearLocation.IsGoldMedalClear(_locations, stageId);
                if (medal == EEstimateTime.Gold && !goldMedalClearedRemotely)
                {
                    GoldMedalClearLocation.ClearGoldMedal(_locations, stageId);
                }
                else if (_gameSaveDataService.GetStageFlag(stageId) == EStageFlag.Clear && medal != EEstimateTime.Gold && goldMedalClearedRemotely)
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
            EPlayerSkin.Sonic,
            EPlayerSkin.Tocman,
            EPlayerSkin.PacLand
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
                bool unlockedInWorld = SkinItem.IsSkinReceived(_unlocks, skin);
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
            int ogLifeCount = _gameSaveDataService.GetStockNum();
            int lives = _pendingLives;
            _pendingLives = 0;

            if (!_activeSceneService.InNonVillageStage || !_playerPacmanStateService.IsInMoveState)
            {
                if (lives > 0)
                {
                    _gameSaveDataService.SetStockNum(ogLifeCount + lives);
                    _syncStockCount(ogLifeCount + lives);
                }
                return;
            }

            ogLifeCount = StageStateManager.CurrentStock;

            if (lives > 0)
            {
                StageStateManager.AddStock(lives);
                _syncStockCount(ogLifeCount + lives);
            }

            int dots = _pendingPacDots;
            _pendingPacDots = 0;
            if (dots > 0)
            {
                StageStateManager.AddPacDot(dots);
            }

            int score = _pendingPoints;
            _pendingPoints = 0;
            if (score > 0)
            {
                StageStateManager.AddScore(EStageScore.Dot, Vector3.zero, score);
            }
        }

        private void _syncStockCount(int count)
        {
            var lifeGauge = GameUI.LifeGauge;
            if (lifeGauge == null)
            {
                return;
            }

            lifeGauge.SetStock(count, Vector3.zero);
        }

        public void _onReceivePacDots(int count)
        {
            _pendingPacDots += count;
        }

        public void _onReceiveLives(int count)
        {
            _pendingLives += count;
        }

        public void _onReceivePoints(int count)
        {
            _pendingPoints += count;
        }
    }
}
