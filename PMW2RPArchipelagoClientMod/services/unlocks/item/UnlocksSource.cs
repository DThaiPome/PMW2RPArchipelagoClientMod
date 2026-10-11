using Archipelago.MultiClient.Net.Models;
using MelonLoader;
using PMW2RPArchipelagoClientMod.services.client;
using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using PMW2RPArchipelagoClientMod.services.unlocks.registry;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public class UnlocksSource : IUnlocksService, IItemsDispatcher
    {
        private MelonMod _melonMod;
        private IAPConnectionService _apConnectionService;
        private IIdMapperService _idMapperService;

        private Dictionary<IUnlockableItem, int> _permanentUnlockCounts = new Dictionary<IUnlockableItem, int>();
        private Queue<IUnlockableConsumable> _pendingConsumables = new Queue<IUnlockableConsumable>();
        private ConsumableDelegates _consumablesDelegates = new ConsumableDelegates();

        private bool _shouldBlockConsumables;

        public IConsumablesDelegates ConsumablesDelegates => _consumablesDelegates;

        public UnlocksSource(MelonMod melonMod,
            IAPConnectionService apConnectioNService,
            IIdMapperService idMapperService)
        {
            _melonMod = melonMod;
            _apConnectionService = apConnectioNService;

            _apConnectionService.InitItems += _onInitItems;
            _apConnectionService.ItemReceived += _onItemReceived;
            _idMapperService = idMapperService;
        }

        private void _onInitItems(IReadOnlyList<ItemInfo> items)
        {
            _resetUnlocks();
            _shouldBlockConsumables = true;
            foreach (var item in items)
            {
                _unlockItem(item);
            }
            _shouldBlockConsumables = false;
        }

        private void _onItemReceived(ItemInfo item)
        {
            _unlockItem(item);
        }

        private void _unlockItem(ItemInfo item)
        {
            long id = item.ItemId;
            if (_idMapperService.TryGetItemFromId(id, out var i))
            {
                i.Unlock(this);
            }
            else if (!_shouldBlockConsumables && _idMapperService.TryGetConsumableFromId(id, out var consumable))
            {
                _pendingConsumables.Enqueue(consumable);
            }
        }

        private void _resetUnlocks()
        {
            _permanentUnlockCounts.Clear();
            _pendingConsumables.Clear();
        }

        public int GetCountReceived(IUnlockableItem item)
        {
            return _permanentUnlockCounts.GetValueOrDefault(item, 0);
        }

        public bool IsUnlocked(IUnlockableItem item)
        {
            return _permanentUnlockCounts.ContainsKey(item);
        }

        public void ReceiveItem(IUnlockableItem item)
        {
            item.Unlock(this);
        }

        public void UnlockPermanently(IUnlockableItem item)
        {
            if (_permanentUnlockCounts.ContainsKey(item))
            {
                _permanentUnlockCounts[item]++;
            }
            else
            {
                _permanentUnlockCounts.Add(item, 1);
            }
        }

        public void FlushConsumables()
        {
            foreach (var consumable in _pendingConsumables)
            {
                consumable.Consume(_consumablesDelegates);
            }
            _pendingConsumables.Clear();
        }

        public void RescindItem(IUnlockableItem item)
        {
            // Nope
        }

        public void OnLateUpdate()
        {
            FlushConsumables();
        }

        public void ReceiveConsumable(IUnlockableConsumable consumableId)
        {
            _pendingConsumables.Enqueue(consumableId);
        }
    }
}
