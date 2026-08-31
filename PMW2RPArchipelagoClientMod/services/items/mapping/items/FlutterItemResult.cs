namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class FlutterItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.Flutter = true;
        }
    }
}
