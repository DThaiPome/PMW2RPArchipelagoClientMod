namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class PacDotItemResult : IItemMapEntry
    {
        private int _count;

        public PacDotItemResult(int count)
        {
            _count = count;
        }
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.GivePacDots(_count);
        }
    }
}
