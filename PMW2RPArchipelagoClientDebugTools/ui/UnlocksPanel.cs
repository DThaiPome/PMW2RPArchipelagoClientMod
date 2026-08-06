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

namespace PMW2RPArchipelagoClientDebugTools.ui
{
    public class UnlocksPanel : PanelBase
    {
        private Toggle _buttBounceToggle1;
        private Toggle _buttBounceToggle2;
        private Toggle _kickToggle;
        private Toggle _dashToggle;
        private Toggle _bombToggle;

        public UnlocksPanel(UIBase owner) : base(owner)
        {

        }

        public override string Name => "Toggle Unlocks";

        public override int MinWidth => 300;

        public override int MinHeight => 600;

        public override Vector2 DefaultAnchorMin => new Vector2(0f, 0f);

        public override Vector2 DefaultAnchorMax => new Vector2(0f, 0f);

        protected override void ConstructPanelContent()
        {
            ConstructToggle("buttBounce1", "Progressive Butt-Bounce", out _buttBounceToggle1);
            ConstructToggle("buttBounce2", "Progressive Butt-Bounce", out _buttBounceToggle2);
            ConstructToggle("kick", "Flip Kick", out _kickToggle);
            ConstructToggle("dash", "Dash", out _dashToggle);
            ConstructToggle("bomb", "Pac-Dot Throw", out _bombToggle);

            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.GetDebugUnlocksService();
            _kickToggle.isOn = debugUnlocksService.FlipKick;
            _dashToggle.isOn = debugUnlocksService.Dash;
            _bombToggle.isOn = debugUnlocksService.Bomb;

            _buttBounceToggle1.isOn = debugUnlocksService.ButtBounce != ProgressiveButtBounce.None;
            _buttBounceToggle2.isOn = debugUnlocksService.ButtBounce == ProgressiveButtBounce.SuperButtBounce;
        }

        private void ConstructToggle(string name, string label, out Toggle toggle)
        {
            Text text;
            UIFactory.CreateToggle(ContentRoot, name, out toggle, out text);
            text.text = label;
            text.fontSize = 24;
        }

        public void UIUpdate()
        {
            var debugUnlocksService = PMW2RPArchipelagoClientMod.services.ServiceFactory.GetDebugUnlocksService();
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
    }
}
