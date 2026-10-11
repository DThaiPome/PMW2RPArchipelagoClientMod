using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public class DebugUnlocksSource : IUnlocksService
    {
        private Dictionary<IUnlockableItem, int> _unlockCounts = new Dictionary<IUnlockableItem, int>();
        private Queue<IUnlockableConsumable> _pendingConsumables = new Queue<IUnlockableConsumable>();

        private ConsumableDelegates _consumableDelegates = new ConsumableDelegates();

        public IConsumablesDelegates ConsumablesDelegates => _consumableDelegates;

        public void FlushConsumables()
        {
            foreach (var consumable in _pendingConsumables)
            {
                consumable.Consume(_consumableDelegates);
            }
            _pendingConsumables.Clear();
        }

        public int GetCountReceived(IUnlockableItem item)
        {
            return _unlockCounts.GetValueOrDefault(item, 0);
        }

        public bool IsUnlocked(IUnlockableItem item)
        {
            return _unlockCounts.ContainsKey(item);
        }

        public void OnLateUpdate()
        {
            FlushConsumables();
        }

        public void ReceiveConsumable(IUnlockableConsumable consumableId)
        {
            _pendingConsumables.Enqueue(consumableId);
        }

        public void ReceiveItem(IUnlockableItem item)
        {
            if (_unlockCounts.ContainsKey(item))
            {
                _unlockCounts[item]++;
            }
            else
            {
                _unlockCounts.Add(item, 1);
            }
        }

        public void RescindItem(IUnlockableItem item)
        {
            if (!_unlockCounts.ContainsKey(item))
            {
                return;
            }
            _unlockCounts[item]--;
            if (_unlockCounts[item] == 0)
            {
                _unlockCounts.Remove(item);
            }
        }
    }
}
