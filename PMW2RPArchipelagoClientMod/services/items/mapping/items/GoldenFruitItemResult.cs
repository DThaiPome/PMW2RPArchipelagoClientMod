using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class GoldenFruitItemResult : IItemMapEntry
    {
        private GoldenFruitItem _goldenFruit;

        public GoldenFruitItemResult(GoldenFruitItem item)
        {
            _goldenFruit = item;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.GoldenFruitMutable.Add(_goldenFruit);
        }
    }
}
