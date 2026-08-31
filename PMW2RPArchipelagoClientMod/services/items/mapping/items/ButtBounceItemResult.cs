namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class ButtBounceItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            if (unlocks.ButtBounce == models.data.ProgressiveButtBounce.SuperButtBounce)
            {
                return;
            }
            unlocks.ButtBounce++;
        }
    }
}
