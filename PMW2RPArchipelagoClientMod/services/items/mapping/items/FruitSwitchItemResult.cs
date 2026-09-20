using Il2Cpp;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class FruitSwitchItemResult : IItemMapEntry
    {
        private EFruits _fruit;

        public FruitSwitchItemResult(EFruits fruit)
        {
            _fruit = fruit;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.FruitSwitchesMutable.Add(_fruit);
        }
    }
}
