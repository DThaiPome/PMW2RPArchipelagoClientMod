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
    }
}
