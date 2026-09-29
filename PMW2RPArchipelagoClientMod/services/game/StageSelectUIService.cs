using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items;
using IUnlocksSource = PMW2RPArchipelagoClientMod.services.items.v2.item.IUnlocksSource;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class StageSelectUIService
    {
        private MelonMod _melonMod;
        private IUnlocksSource _unlocks;
        private IAPConnectionService _apConnectionService;
        private IGameSaveDataService _gameSaveDataService;

        public StageSelectUIService(MelonMod melonMod,
            IUnlocksSource unlocks,
            IAPConnectionService apConnectionService,
            IGameSaveDataService gameSaveDataService)
        {
            _melonMod = melonMod;
            _unlocks = unlocks;
            _apConnectionService = apConnectionService;
            _gameSaveDataService = gameSaveDataService;
        }

        public void OnLateUpdate()
        {
            _updateMapUIIfAble();
        }

        private void _updateMapUIIfAble()
        {
            bool isLevelRando = _apConnectionService.IsLevelRando ?? true;

            StageSelectMapUI mapUI = StageSelectMapUI.Instance;
            if (mapUI == null || mapUI.m_iconList == null)
            {
                return;
            }

            foreach (StageSelectAreaIconUI icon in mapUI.m_iconList)
            {
                EWorldStage stage = (EWorldStage?)icon.StageRoot?.StageInfo?.stageId ?? EWorldStage.PacVillage;
                if (stage == EWorldStage.PacVillage)
                {
                    continue;
                }
                bool stageUnlocked = isLevelRando ? StageItem.IsStageReceived(_unlocks, stage) : _gameSaveDataService.GetStageFlag(stage) != EStageFlag.Locked;
                if (stageUnlocked)
                {
                    continue;
                }
                icon.SetAnimatorState(StageSelectAreaIconUI.EAnimatorState.DISABLED);
            }
        }
    }
}
