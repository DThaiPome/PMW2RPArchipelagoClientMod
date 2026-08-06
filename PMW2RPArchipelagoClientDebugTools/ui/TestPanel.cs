using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Panels;

namespace PMW2RPArchipelagoClientDebugTools.ui
{
    public class TestPanel : PanelBase
    {
        public TestPanel(UIBase owner) : base(owner)
        {
        }

        public override string Name => "TestPanel";

        public override int MinWidth => 10;

        public override int MinHeight => 19;

        public override Vector2 DefaultAnchorMin => new Vector2(0.35f, 0.35f);

        public override Vector2 DefaultAnchorMax => new Vector2(0.65f, 0.65f);

        protected override void ConstructPanelContent()
        {
            var testLabel = UIFactory.CreateLabel(ContentRoot, "TestLabel", "IM A TEST LABEL AHHHH", TextAnchor.MiddleCenter, Color.red, false, 96);
            UIFactory.SetLayoutElement(testLabel.gameObject, minWidth: 100, minHeight: 100, flexibleHeight: 500, flexibleWidth: 500);
        }
    }
}
