namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class ExtraLifeItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.GiveLife();
        }
    }
}
