using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items.mapping;
using System.Collections.Immutable;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public class UnlocksService : IUnlocksSourceMutable
    {
        private MelonMod _melonMod;
        private IAPConnectionService _connectionService;
        private ICheckIdMapperService _itemIdMapperService;

        private Dictionary<EWorldStage, bool> _stages = new Dictionary<EWorldStage, bool>();
        private HashSet<GoldenFruitItem> _goldenFruit = new HashSet<GoldenFruitItem>();
        private HashSet<PastKeyItem> _pastKeys = new HashSet<PastKeyItem>();
        private HashSet<EPlayerSkin> _skins = new HashSet<EPlayerSkin>();
        private HashSet<EFruits> _fruitsSwitches = new HashSet<EFruits>();

        private int _pendingPacDots = 0;
        private int _pendingPoints = 0;

        private bool _unlocksInitialized = false;

        public bool FlipKick { get; set; }

        public bool Dash { get; set; }

        public bool Bomb { get; set; }

        public bool Flutter { get; set; }

        public ProgressiveButtBounce ButtBounce { get; set; }

        public ProgressiveDolphinKick DolphinKick { get; set; }

        public IImmutableDictionary<EWorldStage, bool> Stages
        {
            get
            {
                return _stages.ToImmutableDictionary();
            }
        }

        public IDictionary<EWorldStage, bool> StagesMutable => _stages;

        public IImmutableSet<GoldenFruitItem> GoldenFruit => _goldenFruit.ToImmutableHashSet();
        public ISet<GoldenFruitItem> GoldenFruitMutable => _goldenFruit;

        public IImmutableSet<PastKeyItem> PastKeys => _pastKeys.ToImmutableHashSet();
        public ISet<PastKeyItem> PastKeysMutable => _pastKeys;

        public IImmutableSet<EPlayerSkin> Skins => _skins.ToImmutableHashSet();
        public ISet<EPlayerSkin> SkinsMutable => _skins;

        public IImmutableSet<EFruits> FruitSwitches => _fruitsSwitches.ToImmutableHashSet();
        public ISet<EFruits> FruitSwitchesMutable => _fruitsSwitches;

        public UnlocksService(MelonMod melonMod,
            IAPConnectionService connectionService,
            ICheckIdMapperService itemIdMapperService)
        {
            _melonMod = melonMod;
            _connectionService = connectionService;
            _itemIdMapperService = itemIdMapperService;

            _connectionService.InitItems += InitItems;
            _connectionService.ItemReceived += ItemReceived;

            _clearUnlocks();
        }

        public void InitItems(IReadOnlyList<ItemInfo> items)
        {
            _clearUnlocks();
            foreach (var item in items)
            {
                _itemIdMapperService.MapItem(item).Unlock(this);
            }
            _unlocksInitialized = true;
        }

        public void InitLocations(IReadOnlyList<long> locationIds)
        {

        }

        public void ItemReceived(ItemInfo item)
        {
            _itemIdMapperService.MapItem(item).Unlock(this);
        }

        private void _clearUnlocks()
        {
            FlipKick = false;
            Dash = false;
            Bomb = false;
            Flutter = false;
            ButtBounce = ProgressiveButtBounce.None;
            DolphinKick = ProgressiveDolphinKick.None;
            _stages.Clear();
            _goldenFruit.Clear();
            _pastKeys.Clear();
            _skins.Clear();
            _unlocksInitialized = false;
        }

        public void OnLateUpdate()
        {

        }

        public void GivePacDots(int count)
        {
            if (!_unlocksInitialized)
            {
                return;
            }
            _pendingPacDots += count;
        }

        public void GivePoints(int count)
        {
            if (!_unlocksInitialized)
            {
                return;
            }
            _pendingPoints += count;
        }

        public int FlushPacDots()
        {
            int count = _pendingPacDots;
            _pendingPacDots = 0;
            return count;
        }

        public int FlushPoints()
        {
            int count = _pendingPoints;
            _pendingPoints = 0;
            return count;
        }
    }
}
