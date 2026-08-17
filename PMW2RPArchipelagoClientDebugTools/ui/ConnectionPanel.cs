using PMW2RPArchipelagoClientDebugTools.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;

namespace PMW2RPArchipelagoClientDebugTools.ui
{
    public class ConnectionPanel : PanelBase
    {
        private GameObject _uiRoot;
        private string _domainInput = "localhost";
        private string _portInput = "38281";
        private string _slotInput = "DThaiPome_PMW2RP";

        public ConnectionPanel(UIBase owner) : base(owner)
        {

        }

        public override string Name => "Toggle Unlocks";

        public override int MinWidth => 900;

        public override int MinHeight => 600;

        public override Vector2 DefaultAnchorMin => new Vector2(0f, 0f);

        public override Vector2 DefaultAnchorMax => new Vector2(0f, 0f);

        protected override void ConstructPanelContent()
        {
            _uiRoot = UIFactory.CreateUIObject("connectionPanelRoot", ContentRoot);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(_uiRoot, childControlWidth: true, childControlHeight: true, forceWidth: true, forceHeight: true);
            UIFactory.CreateInputField(_uiRoot, "domainInput", "localhost").OnValueChanged += _onDomainInputChanged;
            UIFactory.CreateInputField(_uiRoot, "portInput", "38281").OnValueChanged += _onPortInputChanged;
            UIFactory.CreateInputField(_uiRoot, "slotInput", "DThaiPome_PMW2RP").OnValueChanged += _onSlotInputChanged;
            UIFactory.CreateButton(_uiRoot, "connectButton", "Connect", ColorBlock.defaultColorBlock).OnClick += _onClick;
        }

        private void _onDomainInputChanged(string value)
        {
            _domainInput = value;
        } 

        private void _onPortInputChanged(string value)
        {
            _portInput = value;
        }

        private void _onSlotInputChanged(string value)
        {
            _slotInput = value;
        }

        private void _onClick()
        {
            if (int.TryParse(_portInput, out int port))
            {
                PMW2RPArchipelagoClientMod.services.ServiceFactory.APConnectionService.CreateSessionAndLogIn(_domainInput, port, _slotInput);
            }
        }
    }
}
