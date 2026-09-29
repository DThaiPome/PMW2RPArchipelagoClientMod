using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Panels;
using PMW2RPArchipelagoClientMod.models.data;
using Il2Cpp;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using IUnlocksSourceMutable = PMW2RPArchipelagoClientMod.services.items.v2.item.IUnlocksSourceMutable;
using PMW2RPArchipelagoClientMod.services.items.v2.item.consumables;

namespace PMW2RPArchipelagoClientDebugTools.ui
{
    public class UnlocksPanel : PanelBase
    {
        private Toggle _buttBounceToggle1;
        private Toggle _buttBounceToggle2;
        private Toggle _dolphinKickToggle1;
        private Toggle _dolphinKickToggle2;
        private Toggle _kickToggle;
        private Toggle _dashToggle;
        private Toggle _bombToggle;
        private Toggle _flutterToggle;

        private GameObject _uiRoot;

        private Dictionary<EWorldStage, Toggle> _stageToggles = new Dictionary<EWorldStage, Toggle>();
        private Dictionary<EFruits, Toggle> _goldenFruitToggles = new Dictionary<EFruits, Toggle>();
        private Dictionary<PastKeyKind, Toggle> _pastKeyToggles = new Dictionary<PastKeyKind, Toggle>();

        public UnlocksPanel(UIBase owner) : base(owner)
        {

        }

        public override string Name => "Toggle Unlocks";

        public override int MinWidth => 1033;

        public override int MinHeight => 1050;

        public override Vector2 DefaultAnchorMin => new Vector2(0f, 0f);

        public override Vector2 DefaultAnchorMax => new Vector2(0f, 0f);

        protected override void ConstructPanelContent()
        {
            _uiRoot = UIFactory.CreateUIObject("unlockPanelRoot", ContentRoot);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(_uiRoot, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: true);
            _constructMovesetToggles();
            _constructStageToggles();
            _constructKeyToggles();
            _constructFillerButtons();
        }

        private void _constructMovesetToggles()
        {
            var columnObj = UIFactory.CreateUIObject("movesetColumn", _uiRoot);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(columnObj, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: false);
            _constructToggle(columnObj, "buttBounce1", "Progressive Butt-Bounce", out _buttBounceToggle1);
            _constructToggle(columnObj, "buttBounce2", "Progressive Butt-Bounce", out _buttBounceToggle2);
            _constructToggle(columnObj, "dolphinKick1", "Progressive Dolphin Kick", out _dolphinKickToggle1);
            _constructToggle(columnObj, "dolphinKick2", "Progressive Dolphin Kick", out _dolphinKickToggle2);
            _constructToggle(columnObj, "kick", "Flip Kick", out _kickToggle);
            _constructToggle(columnObj, "dash", "Dash", out _dashToggle);
            _constructToggle(columnObj, "bomb", "Pac-Dot Throw", out _bombToggle);
            _constructToggle(columnObj, "flutter", "Flutter", out _flutterToggle);

            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            _kickToggle.isOn = MovesetItem.IsFlipKickReceived(debugUnlocksService);
            _dashToggle.isOn = MovesetItem.IsRevRollReceived(debugUnlocksService);
            _bombToggle.isOn = MovesetItem.IsDotThrowReceived(debugUnlocksService);
            _flutterToggle.isOn = MovesetItem.IsFlutterReceived(debugUnlocksService);

            var buttBounceLevel = MovesetItem.GetButtBounceLevel(debugUnlocksService);
            var dolphinKickLevel = MovesetItem.GetDolphinKickLevel(debugUnlocksService);
            _buttBounceToggle1.isOn = buttBounceLevel != ProgressiveButtBounce.None;
            _buttBounceToggle2.isOn = buttBounceLevel == ProgressiveButtBounce.SuperButtBounce;
            _dolphinKickToggle1.isOn = dolphinKickLevel != ProgressiveDolphinKick.None;
            _dolphinKickToggle2.isOn = dolphinKickLevel == ProgressiveDolphinKick.SuperDolphinKick;
        }

        private void _constructStageToggles()
        {
            var columnObj = UIFactory.CreateUIObject("movesetColumn", _uiRoot);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(columnObj, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: false);
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                _constructToggle(columnObj, stage.ToString(), stage.ToString(), out Toggle toggle);
                bool unlocked = StageItem.IsStageReceived(debugUnlocksService, stage);
                toggle.isOn = unlocked;
                _stageToggles.Add(stage, toggle);
            }
        }

        private void _constructKeyToggles()
        {
            var columnObj = UIFactory.CreateUIObject("keyColumn", _uiRoot);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(columnObj, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: false);
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (EFruits item = EFruits.Cherry; item < EFruits.MAX; item++)
            {
                _constructToggle(columnObj, item.ToString(), item.ToString(), out Toggle toggle);
                bool unlocked = GoldenFruitItem.IsGoldenFruitReceived(debugUnlocksService, item);
                toggle.isOn = unlocked;
                _goldenFruitToggles.Add(item, toggle);
            }
            for (PastKeyKind item = PastKeyKind.WindyWoodsKey; item < PastKeyKind.MAX; item++)
            {
                _constructToggle(columnObj, item.ToString(), item.ToString(), out Toggle toggle);
                bool unlocked = PastKeyItem.IsPastKeyReceived(debugUnlocksService, item);
                toggle.isOn = unlocked;
                _pastKeyToggles.Add(item, toggle);
            }
        }

        private void _constructFillerButtons()
        {
            var columnObj = UIFactory.CreateUIObject("fillerColumn", _uiRoot);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(columnObj, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: false);
            ColorBlock colorBlock = ColorBlock.defaultColorBlock;
            colorBlock.m_NormalColor = Color.gray;
            colorBlock.m_HighlightedColor = Color.black;
            UIFactory.CreateButton(_uiRoot, "dotButton", "Give Pac Dot", colorBlock).OnClick += _givePacDotClick;
            UIFactory.CreateButton(_uiRoot, "pointButton", "Give Point", colorBlock).OnClick += _givePointClick;
            UIFactory.CreateButton(_uiRoot, "lifeButton", "Give Life", colorBlock).OnClick += _giveLifeClick;
            UIFactory.CreateButton(_uiRoot, "vlTrapButton", "Give Voice Line Trap", colorBlock).OnClick += _giveVoiceLineTrap;
        }

        private void _givePacDotClick()
        {
            PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService.ReceiveConsumable(new PacDotBundle(FillerKind.PacDot1));
        }

        private void _givePointClick()
        {
            PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService.ReceiveConsumable(new PointsBundle(FillerKind.Points100));
        }

        private void _giveLifeClick()
        {
            PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService.ReceiveConsumable(new ExtraLifeItem());
        }

        private void _giveVoiceLineTrap()
        {
            PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService.ReceiveConsumable(new VoiceLineTrap());
        }

        private void _constructToggle(GameObject parent, string name, string label, out Toggle toggle)
        {
            var toggleObj = UIFactory.CreateToggle(parent, name, out toggle, out Text text);
            UIFactory.SetLayoutElement(toggleObj, minHeight: 10);
            text.text = label;
            text.fontSize = 24;
        }

        public void UIUpdate()
        {
            _updateMoveset();
            _updateLevels();
            _updateKeys();
        }

        private void _updateMoveset()
        {
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            _toggleUnlock(debugUnlocksService, _kickToggle.isOn, MovesetItem.MOVESET_ITEMS[MovesetKind.FlipKick]);
            _toggleUnlock(debugUnlocksService, _dashToggle.isOn, MovesetItem.MOVESET_ITEMS[MovesetKind.RevRoll]);
            _toggleUnlock(debugUnlocksService, _bombToggle.isOn, MovesetItem.MOVESET_ITEMS[MovesetKind.PacDotAttack]);
            _toggleUnlock(debugUnlocksService, _flutterToggle.isOn, MovesetItem.MOVESET_ITEMS[MovesetKind.Flutter]);

            int buttBounceCount = _buttBounceToggle1.isOn ? 1 : 0;
            if (_buttBounceToggle2.isOn)
            {
                buttBounceCount++;
            }
            _bringUnlockCountToTarget(debugUnlocksService, buttBounceCount, MovesetItem.MOVESET_ITEMS[MovesetKind.ProgressiveButtBounce]);

            int dolphinKickCount = _dolphinKickToggle1.isOn ? 1 : 0;
            if (_dolphinKickToggle2.isOn)
            {
                dolphinKickCount++;
            }
            _bringUnlockCountToTarget(debugUnlocksService, dolphinKickCount, MovesetItem.MOVESET_ITEMS[MovesetKind.ProgressiveDolphinKick]);
        }

        private void _toggleUnlock(IUnlocksSourceMutable unlocks, bool shouldBeUnlocked, IUnlockableItemId item)
        {
            bool itemReceived = unlocks.IsUnlocked(item);
            if (itemReceived && !shouldBeUnlocked)
            {
                unlocks.RescindItem(item);
            }
            else if (!itemReceived && shouldBeUnlocked)
            {
                unlocks.ReceiveItem(item);
            }
        }

        private void _bringUnlockCountToTarget(IUnlocksSourceMutable unlocks, int count, IUnlockableItemId item)
        {
            int ogCount = unlocks.GetCountReceived(item);
            if (ogCount == count)
            {
                return;
            }
            if (ogCount > count)
            {
                for (int i = 0; i < (ogCount - count); i++)
                {
                    unlocks.RescindItem(item);
                }
            }
            else if (ogCount < count)
            {
                for (int i = 0; i < (count - ogCount); i++)
                {
                    unlocks.ReceiveItem(item);
                }
            }
        }

        private void _updateLevels()
        {
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                _toggleUnlock(debugUnlocksService, _stageToggles[stage].isOn, StageItem.STAGE_ITEMS[stage]);
            }
        }
        
        private void _updateKeys()
        {
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (EFruits item = EFruits.Cherry; item < EFruits.MAX; item++)
            {
                bool isOn = _goldenFruitToggles[item].isOn;
                _toggleUnlock(debugUnlocksService, isOn, GoldenFruitItem.GOLDEN_FRUIT_ITEMS[item]);
            }
            for (PastKeyKind item = PastKeyKind.WindyWoodsKey; item < PastKeyKind.MAX; item++)
            {
                bool isOn = _pastKeyToggles[item].isOn;
                _toggleUnlock(debugUnlocksService, isOn, PastKeyItem.PAST_KEY_ITEMS[item]);
            }
        }
    }
}
