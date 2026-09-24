using MelonLoader;
using PMW2RPArchipelagoClientMod.services;

[assembly: MelonInfo(typeof(PMW2RPArchipelagoClientMod.Core), "PMW2RPArchipelagoClientMod", "1.1.1a", "DThaiPome", null)]
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

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            base.OnSceneWasInitialized(buildIndex, sceneName);
            ServiceFactory.GoalCheckVisibilityService.OnSceneWasInitialized();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            ServiceFactory.UnlocksService.OnLateUpdate();
            ServiceFactory.LocationsService.OnLateUpdate();
            ServiceFactory.LevelUnlockSyncService.OnLateUpdate();
            ServiceFactory.ActiveSceneService.OnLateUpdate();
            ServiceFactory.StageDataPatchService.OnLateUpdate();
            ServiceFactory.StageSelectCinematicService.OnLateUpdate();
            ServiceFactory.VoiceLineTrapService.OnLateUpdate();
            ServiceFactory.StageSelectUIService.OnLateUpdate();
            ServiceFactory.GoalCheckVisibilityService.OnLateUpdate();

            // Run this last so that services have a chance to subscribe to events
            ServiceFactory.APConnectionService.OnLateUpdate();
            ServiceFactory.APSessionService.OnLateUpdate();
        }
    }
}