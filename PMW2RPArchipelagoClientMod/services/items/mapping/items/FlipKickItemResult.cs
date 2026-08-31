namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class FlipKickItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.FlipKick = true;
        }
    }
}
