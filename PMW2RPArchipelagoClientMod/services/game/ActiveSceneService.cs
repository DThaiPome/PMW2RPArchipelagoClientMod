using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class ActiveSceneService
    {
        private MelonMod _melonMod;

        private static List<EScene> _outOfGameScenes = new List<EScene>()
        {
            EScene.None,
            EScene.Title,
            EScene.Logo,
            EScene.Movie,
            EScene.Credit,
            EScene.CreditNew,
            EScene.CreditSonic,
            EScene.LoginXbox,
            EScene.LaunchActivity,
            EScene.FirstScene,
            EScene.Dummy
        };

        public ActiveSceneService(MelonMod melonMod)
        {
            _melonMod = melonMod;
        }

        public bool InLoadedSave
        {
            get
            {
                var sceneManagerInstance = SceneManager.Instance;
                if (sceneManagerInstance == null)
                {
                    return false;
                }
                return !_outOfGameScenes.Contains(sceneManagerInstance.m_eCurrentScene);
            }
        }

        public bool OnStageSelect
        {
            get
            {
                var sceneManagerInstance = SceneManager.Instance;
                if (sceneManagerInstance == null)
                {
                    return false;
                }
                return sceneManagerInstance.m_eCurrentScene == EScene.StageSelect || sceneManagerInstance.m_eCurrentScene == EScene.StageSelect_Past;
            }
        }

        public void OnLateUpdate()
        {

        }
    }
}
