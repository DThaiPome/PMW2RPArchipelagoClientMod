using Il2Cpp;
using Il2CppSystem.Linq;

namespace PMW2RPArchipelagoClientMod.models.data
{
    public interface IUnlocks
    {
        public bool FlipKick { get; }
        public bool Dash { get; }
        public bool Bomb { get; }
        public ProgressiveButtBounce ButtBounce { get; }
        // public Set<EWorldStage> Stages { get; }
        // public Set<EArea> Worlds { get; }
    }
}
