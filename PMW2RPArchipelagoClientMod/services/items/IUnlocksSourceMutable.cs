using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public interface IUnlocksSourceMutable : IUnlocksService
    {
        public new bool FlipKick { get; set; }
        public new bool Dash { get; set; }
        public new bool Bomb { get; set; }
        public new bool Flutter { get; set; }
        public new ProgressiveButtBounce ButtBounce { get; set; }
        public new ProgressiveDolphinKick DolphinKick { get; set; }

        public IDictionary<EWorldStage, bool> StagesMutable { get; }
        public ISet<GoldenFruitItem> GoldenFruitMutable { get;}
        public ISet<PastKeyItem> PastKeysMutable { get; }
    }
}
