using MelonLoader;
using PMW2RPArchipelagoClientMod.models;

namespace PMW2RPArchipelagoClientMod.services
{
    public class ServiceFactory
    {
        private static MelonMod _melonMod = null;
        private static DebugUnlockService _debugUnlockService = null;
        private static PlayerPacmanStateService _playerPacmanStateService = null;

        public static void Init(MelonMod melonMod)
        {
            if (melonMod == null)
            {
                throw new ArgumentNullException("MELON MOD NULL");
            }
            _melonMod = melonMod;
        }

        public static MelonMod GetModInstance()
        {
            if (_melonMod == null)
            {
                throw new InvalidDataException("MELON MOD NULL");
            }
            return _melonMod;
        }

        public static IUnlocksService GetUnlocksService()
        {
            if (_melonMod == null)
            {
                throw new InvalidDataException("MELON MOD NULL");
            }
            if (_debugUnlockService == null)
            {
                _debugUnlockService = new DebugUnlockService(_melonMod);
            }
            return _debugUnlockService;
        }

        public static IUnlocks GetUnlocks()
        {
            return GetUnlocksService();
        }

        public static PlayerPacmanStateService GetPlayerPacmanStateService()
        {
            if (_melonMod == null)
            {
                throw new InvalidDataException("MELON MOD NULL");
            }
            if (_playerPacmanStateService == null)
            {
                _playerPacmanStateService = new PlayerPacmanStateService(_melonMod);
            }
            return _playerPacmanStateService;
        }
    }
}
