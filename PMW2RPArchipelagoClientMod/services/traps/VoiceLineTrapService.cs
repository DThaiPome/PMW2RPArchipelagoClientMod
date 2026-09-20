using Il2CppCriWare;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.items;
using PMW2RPArchipelagoClientMod.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace PMW2RPArchipelagoClientMod.services.traps
{
    public class VoiceLineTrapService
    {
        private MelonMod _melonMod;
        private IUnlocksSource _unlocks;

        private static readonly long DELAY_MS_MIN = 750;
        private static readonly long DELAY_MS_MAX = 2500;

        private static readonly int VOICE_CLIP_MIN = 5;
        private static readonly int VOICE_CLIP_MAX = 10;

        private long _msUntilVoiceLine;
        private long _msSinceVoiceLine;

        private int _voiceClipsRemaining;
        

        public VoiceLineTrapService(MelonMod melonMod, IUnlocksSource unlocks)
        {
            _melonMod = melonMod;
            _unlocks = unlocks;
        }

        private void _resetCooldown()
        {
            _msSinceVoiceLine = TimeUtil.NowMs();
            _msUntilVoiceLine = new System.Random().NextInt64(DELAY_MS_MIN, DELAY_MS_MAX);
        }

        private void _startVoiceClips()
        {
            _voiceClipsRemaining = new System.Random().Next(VOICE_CLIP_MAX - VOICE_CLIP_MIN) + VOICE_CLIP_MIN;
        }

        public void OnLateUpdate()
        {
            _runIfAble();
        }

        private void _runIfAble()
        {
            if (TimeUtil.NowMs() - _msSinceVoiceLine < _msUntilVoiceLine)
            {
                return;
            }
            if (_voiceClipsRemaining > 0)
            {
                _triggerVoiceClip();
            }
            else if (_unlocks.DequeueVoiceLineTrap())
            {
                _startVoiceClips();
            }
        }

        private void _triggerVoiceClip()
        {

            var listenerObj = UnityEngine.Object.FindObjectOfType<CriAtomListener>();
            if (listenerObj == null)
            {
                return;
            }

            CriAtomSource source;
            if (!listenerObj.gameObject.TryGetComponent<CriAtomSource>(out source))
            {
                source = listenerObj.gameObject.AddComponent<CriAtomSource>();
                source.cueSheet = "VO_CUTSCENE";
                source.volume = 3;
                source.use3dPositioning = true;
            }

            CriAtom criAtom = CriAtom.instance;
            if (criAtom == null)
            {
                return;
            }

            var voCueSheet = CriAtom.GetCueSheet("VO_CUTSCENE");
            if (voCueSheet == null)
            {
                return;
            }

            var cues = voCueSheet.acb.GetCueInfoList();
            var cue = cues[new System.Random().Next(cues.Count)];

            source.Play(cue.name);
            _voiceClipsRemaining--;
            _resetCooldown();
        }
    }
}
