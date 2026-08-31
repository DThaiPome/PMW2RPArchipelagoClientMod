using Il2Cpp;
using PMW2RPArchipelagoClientMod.services;
using System.Collections.Immutable;

namespace PMW2RPArchipelagoClientMod.models.data
{
    public interface IUnlocksSource
    {
        public bool FlipKick { get; }
        public bool Dash { get; }
        public bool Bomb { get; }
        public bool Flutter { get; }
        public ProgressiveButtBounce ButtBounce { get; }
        public ProgressiveDolphinKick DolphinKick { get; }
        public IImmutableDictionary<EWorldStage, bool> Stages { get; }
        public IImmutableSet<GoldenFruitItem> GoldenFruit { get; }
        public IImmutableSet<PastKeyItem> PastKeys { get; }

        private static readonly IEnumerable<GoldenFruitItem> _allGoldenFruits = [GoldenFruitItem.GoldenCherry,
            GoldenFruitItem.GoldenStrawberry,
            GoldenFruitItem.GoldenApple,
            GoldenFruitItem.GoldenOrange,
            GoldenFruitItem.GoldenMelon];
        public bool AreAllGoldenFruitsUnlocked()
        {
            return GoldenFruit.SetEquals(_allGoldenFruits);
        }

        private static readonly IEnumerable<PastKeyItem> _allKeys = [PastKeyItem.WindyWoodsKey,
            PastKeyItem.ThunderSnowMountainKey,
            PastKeyItem.FieryCavernsKey,
            PastKeyItem.DimUnderwatersKey,
            PastKeyItem.GhostIslandKey];
        public bool AreAllKeysUnlocked()
        {
            return PastKeys.SetEquals(_allKeys);
        }
    }
}
