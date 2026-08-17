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
        private ConnectionPanel _connectionPanel;

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
            _toggleUnlocksPanel();
            _toggleConnectionPanel();
        }

        private void _toggleUnlocksPanel() {
            if (_unlocksPanel == null)
            {
                _unlocksPanel = new UnlocksPanel(_uiBase);
            }
            else
            {
                _unlocksPanel.Enabled = !_unlocksPanel.Enabled;
            }
        }

        private void _toggleConnectionPanel() {
            if (_connectionPanel == null)
            {
                _connectionPanel = new ConnectionPanel(_uiBase);
            }
            else
            {
                _connectionPanel.Enabled = !_connectionPanel.Enabled;
            }
        }
    }
}
