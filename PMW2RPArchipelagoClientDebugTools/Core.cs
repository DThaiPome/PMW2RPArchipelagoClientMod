using MelonLoader;
using PMW2RPArchipelagoClientDebugTools.services;

[assembly: MelonInfo(typeof(PMW2RPArchipelagoClientDebugTools.Core), "PMW2RPArchipelagoClientDebugTools", "1.0.0", "DThaiPome", null)]
[assembly: MelonGame("Bandai Namco Entertainment Inc.", "PAC-MAN WORLD 2 Re-PAC")]

namespace PMW2RPArchipelagoClientDebugTools
{
    public class Core : MelonPlugin
    {
        public override void OnPreInitialization()
        {
            LoggerInstance.Msg("Pre-initialization.");
        }

        public override void OnInitializeMelon()
        {
            ServiceFactory.Init(this);
            UniverseLib.Universe.Init(0, null, null, new()
            {
                Allow_UI_Selection_Outside_UIBase = true
            });
            LoggerInstance.Msg("Initialized.");
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            ServiceFactory.GetDebugUIService.OnLateUpdate();
        }
    }
}