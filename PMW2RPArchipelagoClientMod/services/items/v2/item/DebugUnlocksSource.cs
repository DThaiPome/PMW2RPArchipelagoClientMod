using PMW2RPArchipelagoClientMod.services.items.v2.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.v2.item
{
    public class DebugUnlocksSource : IUnlocksService
    {
        private Dictionary<IUnlockableItemId, int> _unlockCounts = new Dictionary<IUnlockableItemId, int>();
        private Queue<IUnlockableConsumableId> _pendingConsumables = new Queue<IUnlockableConsumableId>();
        private SingleCallMultiConsumableDispatcher _consumableDispatchers = new SingleCallMultiConsumableDispatcher();

        public void FlushConsumables()
        {
            foreach(var consumable in _pendingConsumables)
            {
                consumable.Consume(_consumableDispatchers);
            }
            _pendingConsumables.Clear();
        }

        public int GetCountReceived(IUnlockableItemId item)
        {
            return _unlockCounts.GetValueOrDefault(item, 0);
        }

        public void GiveConsumableReceiver(IConsumableDispatcher dispatcher)
        {
            _consumableDispatchers.AddDispatcher(dispatcher);
        }

        public bool IsUnlocked(IUnlockableItemId item)
        {
            return _unlockCounts.ContainsKey(item);
        }

        public void OnLateUpdate()
        {
            FlushConsumables();
        }

        public void ReceiveConsumable(IUnlockableConsumableId consumableId)
        {
            _pendingConsumables.Enqueue(consumableId);
        }

        public void ReceiveItem(IUnlockableItemId item)
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

        public void RescindItem(IUnlockableItemId item)
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
