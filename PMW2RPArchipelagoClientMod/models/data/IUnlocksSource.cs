using Il2Cpp;
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
        public IImmutableSet<GoldenFruitKind> GoldenFruit { get; }
        public IImmutableSet<PastKeyKind> PastKeys { get; }
        public IImmutableSet<EPlayerSkin> Skins { get; }
        public IImmutableSet<EFruits> FruitSwitches { get; }

        private static readonly IEnumerable<GoldenFruitKind> _allGoldenFruits = [GoldenFruitKind.GoldenCherry,
            GoldenFruitKind.GoldenStrawberry,
            GoldenFruitKind.GoldenApple,
            GoldenFruitKind.GoldenOrange,
            GoldenFruitKind.GoldenMelon];
        public bool AreAllGoldenFruitsUnlocked()
        {
            return GoldenFruit.SetEquals(_allGoldenFruits);
        }

        private static readonly IEnumerable<PastKeyKind> _allKeys = [PastKeyKind.WindyWoodsKey,
            PastKeyKind.ThunderSnowMountainKey,
            PastKeyKind.FieryCavernsKey,
            PastKeyKind.DimUnderwatersKey,
            PastKeyKind.GhostIslandKey];
        public bool AreAllKeysUnlocked()
        {
            return PastKeys.SetEquals(_allKeys);
        }
        int FlushPacDots();
        int FlushPoints();
        int FlushLives();
        bool DequeueVoiceLineTrap();
    }
}
