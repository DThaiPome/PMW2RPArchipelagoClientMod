namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class VoiceLineTrapItemResult : IItemMapEntry
    {
        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.QueueVoiceLineTrap();
        }
    }
}
