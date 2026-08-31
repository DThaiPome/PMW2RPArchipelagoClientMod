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
            LoggerInstance.Msg("Initialized PMW2RPArchipelagoClientMod.");
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            ServiceFactory.APConnectionService.OnLateUpdate();
            ServiceFactory.UnlocksService.OnLateUpdate();
            ServiceFactory.LocationsService.OnLateUpdate();
            ServiceFactory.LevelUnlockSyncService.OnLateUpdate();
            ServiceFactory.ActiveSceneService.OnLateUpdate();
            ServiceFactory.StageDataPatchService.OnLateUpdate();
            ServiceFactory.StageSelectCinematicService.OnLateUpdate();
        }
    }
}