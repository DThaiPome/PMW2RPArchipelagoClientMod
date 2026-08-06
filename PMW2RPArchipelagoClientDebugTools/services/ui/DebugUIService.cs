using MelonLoader;
using PMW2RPArchipelagoClientDebugTools.ui;
using UnityEngine;
using UnityEngine.InputSystem;
using UniverseLib.UI;

namespace PMW2RPArchipelagoClientDebugTools.services.ui
{
    public class DebugUIService
    {
        private MelonPlugin _melonPlugin;

        private UIBase _uiBase;
        private UnlocksPanel _unlocksPanel;

        public DebugUIService(MelonPlugin melonPlugin)
        {
            _melonPlugin = melonPlugin;
            _uiBase = UniversalUI.RegisterUI("DebugUI", UIUpdate);
        }

        private void UIUpdate()
        {
            if (_unlocksPanel != null)
            {
                _unlocksPanel.UIUpdate();
            }
        }

        public void OnLateUpdate()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                SetUI();
            }
        }

        private void SetUI()
        {
            if (_unlocksPanel == null)
            {
                _melonPlugin.LoggerInstance.Msg("SPAWNING UI");
                _unlocksPanel = new UnlocksPanel(_uiBase);
            }
            else
            {
                _melonPlugin.LoggerInstance.Msg("TOGGLING UI");
                _unlocksPanel.Enabled = !_unlocksPanel.Enabled;
            }
        }
    }
}
