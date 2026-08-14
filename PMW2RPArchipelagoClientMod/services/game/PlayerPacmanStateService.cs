using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class PlayerPacmanStateService
    {
        private MelonMod _melonMod;
        private IUnlocksSource _unlocksSource;

        private bool _skipEndJump;

        private float _scSwimSDKChargeTimeMin;
        private float _scSwimSDKChargeTimeMax;
        private float _scSwimSDK2SDKTimeMin;
        private float _scSwimSDK2SDKTimeMax;
        private float _scSwimDK2SDKTimeMin;
        private float _scSwimDK2SDKTimeMax;
        private bool _superDKSet;

        public PlayerPacmanStateService(MelonMod melonMod,
            IUnlocksSource unlocksSource)
        {
            _melonMod = melonMod;
            _unlocksSource = unlocksSource;

            _skipEndJump = false;
        }

        public void OnLateUpdate()
        {

        }

        public void PushSkipEndJump()
        {
            _skipEndJump = true;
        }

        public bool PopSkipEndJump()
        {
            bool skipEndJump = _skipEndJump;
            _skipEndJump = false;
            return skipEndJump;
        }

        public void SetSuperDK(PlayerPacman __instance)
        {
            _setSuperDKDefaults(__instance);
            if (_unlocksSource.DolphinKick != ProgressiveDolphinKick.SuperDolphinKick)
            {
                __instance.scSwimSDKChargeTimeMin = -1;
                __instance.scSwimSDKChargeTimeMax = -1;
                __instance.scSwimSDK2SDKTimeMin = -1;
                __instance.scSwimSDK2SDKTimeMax = -1;
                __instance.scSwimDK2SDKTimeMin = -1;
                __instance.scSwimDK2SDKTimeMax = -1;
            }
            else
            {
                __instance.scSwimSDKChargeTimeMin = _scSwimSDKChargeTimeMin;
                __instance.scSwimSDKChargeTimeMax = _scSwimSDKChargeTimeMax;
                __instance.scSwimSDK2SDKTimeMin = _scSwimSDK2SDKTimeMin;
                __instance.scSwimSDK2SDKTimeMax = _scSwimSDK2SDKTimeMax;
                __instance.scSwimDK2SDKTimeMin = _scSwimDK2SDKTimeMin;
                __instance.scSwimDK2SDKTimeMax = _scSwimDK2SDKTimeMax;
            }
        }

        private void _setSuperDKDefaults(PlayerPacman __instance)
        {
            if (_superDKSet)
            {
                return;
            }
            _superDKSet = true;
            _scSwimSDKChargeTimeMin = __instance.scSwimSDKChargeTimeMin;
            _scSwimSDKChargeTimeMax = __instance.scSwimSDKChargeTimeMax;
            _scSwimSDK2SDKTimeMin = __instance.scSwimSDK2SDKTimeMin;
            _scSwimSDK2SDKTimeMax = __instance.scSwimSDK2SDKTimeMax;
            _scSwimDK2SDKTimeMin = __instance.scSwimDK2SDKTimeMin;
            _scSwimDK2SDKTimeMax = __instance.scSwimDK2SDKTimeMax;
        }
    }
}
