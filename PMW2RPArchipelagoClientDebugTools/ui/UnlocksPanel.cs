using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Panels;
using PMW2RPArchipelagoClientMod.models.data;
using Il2Cpp;

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
        private Dictionary<GoldenFruitItem, Toggle> _goldenFruitToggles = new Dictionary<GoldenFruitItem, Toggle>();
        private Dictionary<PastKeyItem, Toggle> _pastKeyToggles = new Dictionary<PastKeyItem, Toggle>();

        public UnlocksPanel(UIBase owner) : base(owner)
        {

        }

        public override string Name => "Toggle Unlocks";

        public override int MinWidth => 800;

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
            _kickToggle.isOn = debugUnlocksService.FlipKick;
            _dashToggle.isOn = debugUnlocksService.Dash;
            _bombToggle.isOn = debugUnlocksService.Bomb;
            _flutterToggle.isOn = debugUnlocksService.Flutter;

            _buttBounceToggle1.isOn = debugUnlocksService.ButtBounce != ProgressiveButtBounce.None;
            _buttBounceToggle2.isOn = debugUnlocksService.ButtBounce == ProgressiveButtBounce.SuperButtBounce;
            _dolphinKickToggle1.isOn = debugUnlocksService.DolphinKick != ProgressiveDolphinKick.None;
            _dolphinKickToggle2.isOn = debugUnlocksService.DolphinKick == ProgressiveDolphinKick.SuperDolphinKick;
        }

        private void _constructStageToggles()
        {
            var columnObj = UIFactory.CreateUIObject("movesetColumn", _uiRoot);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(columnObj, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: false);
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                _constructToggle(columnObj, stage.ToString(), stage.ToString(), out Toggle toggle);
                bool unlocked = debugUnlocksService.Stages.GetValueOrDefault(stage, false);
                toggle.isOn = unlocked;
                _stageToggles.Add(stage, toggle);
            }
        }

        private void _constructKeyToggles()
        {
            var columnObj = UIFactory.CreateUIObject("keyColumn", _uiRoot);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(columnObj, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: false);
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (GoldenFruitItem item = GoldenFruitItem.GoldenCherry; item < GoldenFruitItem.MAX; item++)
            {
                _constructToggle(columnObj, item.ToString(), item.ToString(), out Toggle toggle);
                bool unlocked = debugUnlocksService.GoldenFruit.Contains(item);
                toggle.isOn = unlocked;
                _goldenFruitToggles.Add(item, toggle);
            }
            for (PastKeyItem item = PastKeyItem.WindyWoodsKey; item < PastKeyItem.MAX; item++)
            {
                _constructToggle(columnObj, item.ToString(), item.ToString(), out Toggle toggle);
                bool unlocked = debugUnlocksService.PastKeys.Contains(item);
                toggle.isOn = unlocked;
                _pastKeyToggles.Add(item, toggle);
            }
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
            debugUnlocksService.FlipKick = _kickToggle.isOn;
            debugUnlocksService.Dash = _dashToggle.isOn;
            debugUnlocksService.Bomb = _bombToggle.isOn;
            debugUnlocksService.Flutter = _flutterToggle.isOn;

            if (_buttBounceToggle1.isOn && _buttBounceToggle2.isOn)
            {
                debugUnlocksService.ButtBounce = ProgressiveButtBounce.SuperButtBounce;
            }
            else if (_buttBounceToggle1.isOn || _buttBounceToggle2.isOn)
            {
                debugUnlocksService.ButtBounce = ProgressiveButtBounce.ButtBounce;
            }
            else
            {
                debugUnlocksService.ButtBounce = ProgressiveButtBounce.None;
            }

            if (_dolphinKickToggle1.isOn && _dolphinKickToggle2.isOn)
            {
                debugUnlocksService.DolphinKick = ProgressiveDolphinKick.SuperDolphinKick;
            }
            else if (_dolphinKickToggle1.isOn || _dolphinKickToggle2.isOn)
            {
                debugUnlocksService.DolphinKick = ProgressiveDolphinKick.DolphinKick;
            }
            else
            {
                debugUnlocksService.DolphinKick = ProgressiveDolphinKick.None;
            }
        }

        private void _updateLevels()
        {
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                debugUnlocksService.StagesMutable[stage] = _stageToggles[stage].isOn;
            }
        }
        
        private void _updateKeys()
        {
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (GoldenFruitItem item = GoldenFruitItem.GoldenCherry; item < GoldenFruitItem.MAX; item++)
            {
                bool isOn = _goldenFruitToggles[item].isOn;
                if (isOn && !debugUnlocksService.GoldenFruit.Contains(item))
                {
                    PMW2RPArchipelagoClientMod.services.ServiceFactory.ModInstance.LoggerInstance.Msg("GOLDEN FRUIT UNLOCKED: " + item);
                    debugUnlocksService.GoldenFruitMutable.Add(item);
                }
                else if (!isOn && debugUnlocksService.GoldenFruit.Contains(item))
                {
                    PMW2RPArchipelagoClientMod.services.ServiceFactory.ModInstance.LoggerInstance.Msg("GOLDEN FRUIT LOCKED: " + item);
                    debugUnlocksService.GoldenFruitMutable.Remove(item);
                }
            }
            for (PastKeyItem item = PastKeyItem.WindyWoodsKey; item < PastKeyItem.MAX; item++)
            {
                bool isOn = _pastKeyToggles[item].isOn;
                if (isOn && !debugUnlocksService.PastKeys.Contains(item))
                {
                    debugUnlocksService.PastKeysMutable.Add(item);
                }
                else if (!isOn && debugUnlocksService.PastKeys.Contains(item))
                {
                    debugUnlocksService.PastKeysMutable.Remove(item);
                }
            }
        }
    }
}
