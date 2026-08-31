namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class BombItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.Bomb = true;
        }
    }
}
