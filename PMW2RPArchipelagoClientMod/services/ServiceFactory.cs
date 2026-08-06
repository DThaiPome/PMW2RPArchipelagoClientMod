using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.game;
using PMW2RPArchipelagoClientMod.services.items;

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

        public static IDebugUnlocksService GetDebugUnlocksService()
        {
            if (_debugUnlockService == null)
            {
                if (_melonMod == null)
                {
                    throw new InvalidDataException("MELON MOD NULL");
                }
                _debugUnlockService = new DebugUnlockService(_melonMod);
            }
            return _debugUnlockService;
        }

        public static IUnlocksService GetUnlocksService()
        {
            return GetDebugUnlocksService();
        }

        public static IUnlocks GetUnlocks()
        {
            return GetUnlocksService();
        }

        public static PlayerPacmanStateService GetPlayerPacmanStateService()
        {
            if (_playerPacmanStateService == null)
            {
                if (_melonMod == null)
                {
                    throw new InvalidDataException("MELON MOD NULL");
                }
                _playerPacmanStateService = new PlayerPacmanStateService(_melonMod);
            }
            return _playerPacmanStateService;
        }
    }
}
