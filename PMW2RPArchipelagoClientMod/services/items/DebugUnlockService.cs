using Il2Cpp;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using System.Collections.Immutable;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public class DebugUnlockService : IDebugUnlocksService
    {
        private MelonMod _melonMod;
        public bool FlipKick { get; set; }
        public bool Dash { get; set; }
        public bool Bomb { get; set; }
        public bool Flutter { get; set; }
        public ProgressiveButtBounce ButtBounce { get; set; }
        public ProgressiveDolphinKick DolphinKick { get; set; }

        public Dictionary<EWorldStage, bool> _stagesUnlocked { get; private set; }
        public IImmutableDictionary<EWorldStage, bool> Stages => _stagesUnlocked.ToImmutableDictionary();
        public IDictionary<EWorldStage, bool> StagesMutable => _stagesUnlocked;

        public DebugUnlockService(MelonMod melonMod)
        {
            _melonMod = melonMod;
            FlipKick = true;
            Dash = true;
            Bomb = true;
            Flutter = true;
            ButtBounce = ProgressiveButtBounce.SuperButtBounce;
            DolphinKick = ProgressiveDolphinKick.SuperDolphinKick;

            _stagesUnlocked = new Dictionary<EWorldStage, bool>();
        }

        public void OnLateUpdate()
        {

        }
    }
}
