using Il2Cpp;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class SkinItemResult : IItemMapEntry
    {
        private EPlayerSkin _skin;

        public SkinItemResult(EPlayerSkin skin)
        {
            _skin = skin;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.SkinsMutable.Add(_skin);
        }
    }
}
