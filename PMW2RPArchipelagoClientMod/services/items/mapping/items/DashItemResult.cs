namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class DashItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.Dash = true;
        }
    }
}
