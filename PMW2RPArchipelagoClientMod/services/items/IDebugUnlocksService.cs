using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public interface IDebugUnlocksService : IUnlocksService
    {
        public new bool FlipKick { get; set; }
        public new bool Dash { get; set; }
        public new bool Bomb { get; set; }
        public new bool Flutter { get; set; }
        public new ProgressiveButtBounce ButtBounce { get; set; }
        public new ProgressiveDolphinKick DolphinKick { get; set; }

        public IDictionary<EWorldStage, bool> StagesMutable { get; }
    }
}
