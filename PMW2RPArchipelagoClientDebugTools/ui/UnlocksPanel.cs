using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Toggle _kickToggle;
        private Toggle _dashToggle;
        private Toggle _bombToggle;

        private GameObject _uiRoot;

        private Dictionary<EWorldStage, Toggle> _stageToggles = new Dictionary<EWorldStage, Toggle>();

        public UnlocksPanel(UIBase owner) : base(owner)
        {

        }

        public override string Name => "Toggle Unlocks";

        public override int MinWidth => 600;

        public override int MinHeight => 1050;

        public override Vector2 DefaultAnchorMin => new Vector2(0f, 0f);

        public override Vector2 DefaultAnchorMax => new Vector2(0f, 0f);

        protected override void ConstructPanelContent()
        {
            _uiRoot = UIFactory.CreateUIObject("unlockPanelRoot", ContentRoot);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(_uiRoot, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: true);
            _constructMovesetToggles();
            _constructStageToggles();
        }

        private void _constructMovesetToggles()
        {
            var columnObj = UIFactory.CreateUIObject("movesetColumn", _uiRoot);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(columnObj, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: false);
            _constructToggle(columnObj, "buttBounce1", "Progressive Butt-Bounce", out _buttBounceToggle1);
            _constructToggle(columnObj, "buttBounce2", "Progressive Butt-Bounce", out _buttBounceToggle2);
            _constructToggle(columnObj, "kick", "Flip Kick", out _kickToggle);
            _constructToggle(columnObj, "dash", "Dash", out _dashToggle);
            _constructToggle(columnObj, "bomb", "Pac-Dot Throw", out _bombToggle);

            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            _kickToggle.isOn = debugUnlocksService.FlipKick;
            _dashToggle.isOn = debugUnlocksService.Dash;
            _bombToggle.isOn = debugUnlocksService.Bomb;

            _buttBounceToggle1.isOn = debugUnlocksService.ButtBounce != ProgressiveButtBounce.None;
            _buttBounceToggle2.isOn = debugUnlocksService.ButtBounce == ProgressiveButtBounce.SuperButtBounce;
        }

        private void _constructStageToggles()
        {
            var columnObj = UIFactory.CreateUIObject("movesetColumn", _uiRoot);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(columnObj, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: false);
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
                _constructToggle(columnObj, stage.ToString(), stage.ToString(), out Toggle toggle);
                bool unlocked = debugUnlocksService.Stages.GetValueOrDefault(stage, false);
                toggle.isOn = unlocked;
                _stageToggles.Add(stage, toggle);
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
        }

        private void _updateMoveset()
        {
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            debugUnlocksService.FlipKick = _kickToggle.isOn;
            debugUnlocksService.Dash = _dashToggle.isOn;
            debugUnlocksService.Bomb = _bombToggle.isOn;
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
        }

        private void _updateLevels()
        {
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.DebugUnlocksService;
            for (EWorldStage stage = EWorldStage.Stage1_1; stage < EWorldStage.StageSonic_1; stage++)
            {
                debugUnlocksService.StagesMutable[stage] = _stageToggles[stage].isOn;
            }
        }
    }
}
