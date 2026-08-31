using Il2Cpp;
using MelonLoader;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class StageSelectCinematicService
    {
        private MelonMod _melonMod;

        private object _xUnlocksLock = new();
        private Queue<EWorldStage> _unlocks = new Queue<EWorldStage>();
        private bool _recordNeedsClearing = false;
        private HashSet<EWorldStage> _unlocksRecord = new HashSet<EWorldStage>();

        public StageSelectCinematicService(MelonMod melonMod)
        {
            _melonMod = melonMod;
        }

        public void OnLateUpdate()
        {
            _clearRecordIfNeeded();
        }

        private void _clearRecordIfNeeded()
        {
            var stageSelectManager = StageSelectManager.ThisInstance;
            if (stageSelectManager == null)
            {
                return;
            }

            if (stageSelectManager.m_initOnFadeStep < StageSelectManager.EInitOnFade.End)
            {
                return;
            }

            lock (_xUnlocksLock)
            {
                if (!_recordNeedsClearing)
                {
                    return;
                }
                _recordNeedsClearing = false;
                _unlocksRecord.Clear();
            }
        }

        public bool EnqueueUnlock(EWorldStage stage)
        {
            lock(_xUnlocksLock)
            {
                if (_recordNeedsClearing)
                {
                    return false;
                }
                _melonMod.LoggerInstance.Msg("QUEUEING FOR UNLOCK: " + stage);
                _unlocksRecord.Add(stage);
                _unlocks.Enqueue(stage);
            }
            return true;
        }

        public bool IsStageQueued(EWorldStage stage)
        {
            lock(_xUnlocksLock)
            {
                return _unlocks.Contains(stage);
            }
        }

        public IEnumerable<EWorldStage> FlushUnlocks()
        {
            List<EWorldStage> stages = new List<EWorldStage>();
            lock (_xUnlocksLock)
            {
                _recordNeedsClearing = true;
                while (_unlocks.Count > 0)
                {
                    stages.Add(_unlocks.Dequeue());
                }
            }
            return stages.AsEnumerable();
        }
    }
}
