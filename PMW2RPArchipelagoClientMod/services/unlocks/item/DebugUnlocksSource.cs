using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public class DebugUnlocksSource : IUnlocksService
    {
        private Dictionary<long, int> _unlockCounts = new Dictionary<long, int>();
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
            return _unlockCounts.GetValueOrDefault(item.Id, 0);
        }

        public bool IsUnlocked(IUnlockableItem item)
        {
            return _unlockCounts.ContainsKey(item.Id);
        }

        public bool IsUnlocked(long id)
        {
            return _unlockCounts.ContainsKey(id);
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
            if (_unlockCounts.ContainsKey(item.Id))
            {
                _unlockCounts[item.Id]++;
            }
            else
            {
                _unlockCounts.Add(item.Id, 1);
            }
        }

        public void RescindItem(IUnlockableItem item)
        {
            if (!_unlockCounts.ContainsKey(item.Id))
            {
                return;
            }
            _unlockCounts[item.Id]--;
            if (_unlockCounts[item.Id] == 0)
            {
                _unlockCounts.Remove(item.Id);
            }
        }
    }
}
