using PMW2RPArchipelagoClientMod.services.items.item.consumables.@base;
using PMW2RPArchipelagoClientMod.services.items.item.items.@base;

namespace PMW2RPArchipelagoClientMod.services.items.item
{
    public class ComboItemSource : IUnlocksSource
    {
        private IUnlocksService _releaseUnlocksSource;
        private IUnlocksService _debugUnlocksSource;

        public ComboItemSource(IUnlocksService releaseUnlocksSource, IUnlocksService debugUnlocksSource)
        {
            _releaseUnlocksSource = releaseUnlocksSource;
            _debugUnlocksSource = debugUnlocksSource;
        }

        public IConsumablesDelegates ConsumablesDelegates => _releaseUnlocksSource.ConsumablesDelegates;

        public int GetCountReceived(IUnlockableItem item)
        {
            return _releaseUnlocksSource.GetCountReceived(item) + _debugUnlocksSource.GetCountReceived(item);
        }

        public bool IsUnlocked(IUnlockableItem item)
        {
            return _releaseUnlocksSource.IsUnlocked(item) || _debugUnlocksSource.IsUnlocked(item);
        }

        public bool IsUnlocked(long id)
        {
            return _releaseUnlocksSource.IsUnlocked(id) || _debugUnlocksSource.IsUnlocked(id);
        }
    }
}
