using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.items.mapping.items
{
    public class GoldenFruitItemResult : IItemMapEntry
    {
        private GoldenFruitKind _goldenFruit;

        public GoldenFruitItemResult(GoldenFruitKind item)
        {
            _goldenFruit = item;
        }

        public void Unlock(IUnlocksSourceMutable unlocks)
        {
            unlocks.GoldenFruitMutable.Add(_goldenFruit);
        }
    }
}
