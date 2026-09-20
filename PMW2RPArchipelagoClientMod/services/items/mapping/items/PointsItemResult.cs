namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class PointsItemResult : IItemMapEntry
    {
        private int _count;

        public PointsItemResult(int count)
        {
            _count = count;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.GivePoints(_count);
        }
    }
}
