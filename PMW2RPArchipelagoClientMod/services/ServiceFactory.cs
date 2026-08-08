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
        private static LevelUnlockSyncService _levelUnlockSyncService = null;

        public static void Init(MelonMod melonMod)
        {
            if (melonMod == null)
            {
                throw new ArgumentNullException("MELON MOD NULL");
            }
            _melonMod = melonMod;
        }

        private static void _assertInit()
        {
            if (_melonMod == null)
            {
                throw new InvalidDataException("MELON MOD NULL");
            }
        }

        public static MelonMod GetModInstance()
        {
            _assertInit();
            return _melonMod;
        }

        public static IDebugUnlocksService GetDebugUnlocksService()
        {
            if (_debugUnlockService == null)
            {
                _assertInit();
                _debugUnlockService = new DebugUnlockService(_melonMod);
            }
            return _debugUnlockService;
        }

        public static IUnlocksService GetUnlocksService()
        {
            return GetDebugUnlocksService();
        }

        public static IUnlocksSource GetUnlocks()
        {
            return GetUnlocksService();
        }

        public static PlayerPacmanStateService GetPlayerPacmanStateService()
        {
            if (_playerPacmanStateService == null)
            {
                _assertInit();
                _playerPacmanStateService = new PlayerPacmanStateService(_melonMod);
            }
            return _playerPacmanStateService;
        }

        public static LevelUnlockSyncService GetLevelUnlockSyncService()
        {
            if (_levelUnlockSyncService == null)
            {
                _assertInit();
                _levelUnlockSyncService = new LevelUnlockSyncService(_melonMod, GetUnlocks());
            }
            return _levelUnlockSyncService;
        }
    }
}
