using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.game;
using PMW2RPArchipelagoClientMod.services.items;
using PMW2RPArchipelagoClientMod.services.items.debug;
using PMW2RPArchipelagoClientMod.services.items.mapping;

namespace PMW2RPArchipelagoClientMod.services
{
    public class ServiceFactory
    {
        private static MelonMod _melonMod = null;
        private static DebugUnlockService _debugUnlockService = null;
        private static UnlocksService _releaseUnlocksService = null;
        private static DebuggableUnlocksService _comboUnlocksService = null;
        private static PlayerPacmanStateService _playerPacmanStateService = null;
        private static LevelUnlockSyncService _levelUnlockSyncService = null;
        private static ActiveSceneService _activeSceneService = null;
        private static IGameSaveDataService _gameSaveDataService = null;
        private static IAPConnectionService _apConnectionService = null;
        private static ICheckIdMapperService _checkIdMapperService = null;
        private static ILocationsService _locationsService = null;

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

        public static IUnlocksSourceMutable DebugUnlocksService
        {
            get
            {
                if (_debugUnlockService == null)
                {
                    _debugUnlockService = new DebugUnlockService(ModInstance);
                }
                return _debugUnlockService;
            }
        }

        public static IUnlocksSourceMutable ReleaseUnlocksService
        {
            get
            {
                if (_releaseUnlocksService == null)
                {
                    _releaseUnlocksService = new UnlocksService(ModInstance, APConnectionService, CheckIdMapperService);
                }
                return _releaseUnlocksService;
            }
        }

        public static IUnlocksService ComboUnlocksService
        {
            get
            {
                if (_comboUnlocksService == null)
                {
                    _comboUnlocksService = new DebuggableUnlocksService(ReleaseUnlocksService, DebugUnlocksService);
                }
                return _comboUnlocksService;
            }
        }

        public static IUnlocksService UnlocksService
        {
            get
            {
                return ComboUnlocksService;
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
                    _playerPacmanStateService = new PlayerPacmanStateService(ModInstance, Unlocks);
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
                    _levelUnlockSyncService = new LevelUnlockSyncService(ModInstance, Unlocks, Locations, GameSaveDataService);
                }
                return _levelUnlockSyncService;
            }
        }

        public static ActiveSceneService ActiveSceneService
        {
            get
            {
                if (_activeSceneService == null)
                {
                    _activeSceneService = new ActiveSceneService(ModInstance);
                }
                return _activeSceneService;
            }
        }

        public static IGameSaveDataService GameSaveDataService
        {
            get
            {
                if (_gameSaveDataService == null)
                {
                    _gameSaveDataService = new GameSaveDataService(ModInstance, ActiveSceneService);
                }
                return _gameSaveDataService;
            }
        }

        public static IAPConnectionService APConnectionService
        {
            get
            {
                if (_apConnectionService == null)
                {
                    _apConnectionService = new APConnectionService(ModInstance);
                }
                return _apConnectionService;
            }
        }

        public static ICheckIdMapperService CheckIdMapperService
        {
            get
            {
                if (_checkIdMapperService == null)
                {
                    _checkIdMapperService = new CheckIdMapperService();
                }
                return _checkIdMapperService;
            }
        }

        public static ILocationsService LocationsService
        {
            get
            {
                if (_locationsService == null)
                {
                    _locationsService = new LocationsService(ModInstance, APConnectionService, CheckIdMapperService);
                }
                return _locationsService;
            }
        }

        public static ILocationsService Locations => LocationsService;
    }
}
