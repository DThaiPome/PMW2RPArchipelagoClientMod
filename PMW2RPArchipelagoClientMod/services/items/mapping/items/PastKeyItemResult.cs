using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class PastKeyItemResult : IItemMapEntry
    {
        private PastKeyItem _pastKey;

        public PastKeyItemResult(PastKeyItem pastKey)
        {
            _pastKey = pastKey;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.PastKeysMutable.Add(_pastKey);
        }
    }
}
