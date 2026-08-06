using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.services;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(PMW2RPArchipelagoClientMod.Core), "PMW2RPArchipelagoClientMod", "1.0.0", "DThaiPome", null)]
[assembly: MelonGame("Bandai Namco Entertainment Inc.", "PAC-MAN WORLD 2 Re-PAC")]

namespace PMW2RPArchipelagoClientMod
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            ServiceFactory.Init(this);
            UniverseLib.Universe.Init();
            LoggerInstance.Msg("Initialized PMW2RPArchipelagoClientMod.");
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            ServiceFactory.GetUnlocksService().OnLateUpdate();
        }
    }
}