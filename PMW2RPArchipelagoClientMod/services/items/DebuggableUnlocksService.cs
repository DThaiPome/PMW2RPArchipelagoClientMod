using Il2Cpp;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.util;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMW2RPArchipelagoClientMod.services.items
{
    public class DebuggableUnlocksService : IUnlocksService
    {
        private IUnlocksService _releaseUnlocksSource;
        private IUnlocksService _debugUnlocksSource;

        private FallbackDictionary<EWorldStage> _stages;

        public DebuggableUnlocksService(IUnlocksService releaseUnlocksSource, IUnlocksService debugUnlocksSource)
        {
            _releaseUnlocksSource = releaseUnlocksSource;
            _debugUnlocksSource = debugUnlocksSource;
            
            _stages = new FallbackDictionary<EWorldStage>(() => _releaseUnlocksSource.Stages, () => _debugUnlocksSource.Stages);
        }

        public bool FlipKick => _releaseUnlocksSource.FlipKick || _debugUnlocksSource.FlipKick;

        public bool Dash => _releaseUnlocksSource.Dash || _debugUnlocksSource.Dash;

        public bool Bomb => _releaseUnlocksSource.Bomb || _debugUnlocksSource.Bomb;

        public bool Flutter => _releaseUnlocksSource.Flutter || _debugUnlocksSource.Flutter;

        public ProgressiveButtBounce ButtBounce => (ProgressiveButtBounce)Math.Max((int)_releaseUnlocksSource.ButtBounce, (int)_debugUnlocksSource.ButtBounce);

        public ProgressiveDolphinKick DolphinKick => (ProgressiveDolphinKick)Math.Max((int)_releaseUnlocksSource.DolphinKick, (int)_debugUnlocksSource.DolphinKick);

        public IImmutableDictionary<EWorldStage, bool> Stages => _stages;

        public void OnLateUpdate()
        {
            _releaseUnlocksSource.OnLateUpdate();
            _debugUnlocksSource.OnLateUpdate();
        }
    }
}
