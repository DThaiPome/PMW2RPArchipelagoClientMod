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

        public static MelonMod ModInstance
        { 
            get {
                _assertInit();
                return _melonMod;
            }
        }

        public static IDebugUnlocksService DebugUnlocksService
        {
            get
            {
                if (_debugUnlockService == null)
                {
                    _assertInit();
                    _debugUnlockService = new DebugUnlockService(_melonMod);
                }
                return _debugUnlockService;
            }
        }

        public static IUnlocksService UnlocksService
        {
            get
            {
                return DebugUnlocksService;
            }
        }

        public static IUnlocksSource Unlocks
        {
            get
            {
                return UnlocksService;
            }
        }

        public static PlayerPacmanStateService PlayerPacmanStateService
        {
            get
            {
                if (_playerPacmanStateService == null)
                {
                    _assertInit();
                    _playerPacmanStateService = new PlayerPacmanStateService(_melonMod);
                }
                return _playerPacmanStateService;
            }
        }

        public static LevelUnlockSyncService LevelUnlockSyncService
        {
            get
            {
                if (_levelUnlockSyncService == null)
                {
                    _assertInit();
                    _levelUnlockSyncService = new LevelUnlockSyncService(_melonMod, Unlocks);
                }
                return _levelUnlockSyncService;
            }
        }
    }
}
