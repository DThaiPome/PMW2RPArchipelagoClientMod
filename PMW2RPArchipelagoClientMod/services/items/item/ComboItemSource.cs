using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public class ComboItemSource : IUnlocksService
    {
        private IUnlocksService _releaseUnlocksSource;
        private IUnlocksService _debugUnlocksSource;

        public ComboItemSource(IUnlocksService releaseUnlocksSource, IUnlocksService debugUnlocksSource)
        {
            _releaseUnlocksSource = releaseUnlocksSource;
            _debugUnlocksSource = debugUnlocksSource;
        }

        public void FlushConsumables()
        {
            _releaseUnlocksSource.FlushConsumables();
            _debugUnlocksSource.FlushConsumables();
        }

        public int GetCountReceived(IUnlockableItemId item)
        {
            return _releaseUnlocksSource.GetCountReceived(item) + _debugUnlocksSource.GetCountReceived(item);
        }

        public void GiveConsumableReceiver(IConsumableDispatcher dispatcher)
        {
            _releaseUnlocksSource.GiveConsumableReceiver(dispatcher);
            _debugUnlocksSource.GiveConsumableReceiver(dispatcher);
        }

        public bool IsUnlocked(IUnlockableItemId item)
        {
            return _releaseUnlocksSource.IsUnlocked(item) || _debugUnlocksSource.IsUnlocked(item);
        }

        public void OnLateUpdate()
        {
            FlushConsumables();
        }

        public void ReceiveConsumable(IUnlockableConsumableId consumableId)
        {
            _releaseUnlocksSource.ReceiveConsumable(consumableId);
        }

        public void ReceiveItem(IUnlockableItemId item)
        {
            _releaseUnlocksSource.ReceiveItem(item);
        }

        public void RescindItem(IUnlockableItemId item)
        {

        }
    }
}
