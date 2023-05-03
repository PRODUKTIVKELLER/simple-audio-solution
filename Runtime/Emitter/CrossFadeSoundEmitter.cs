using System;
using System.Collections.Generic;
using Produktivkeller.SimpleAudioSolution.Access;
using Produktivkeller.SimpleAudioSolution.Event;
using Produktivkeller.SimpleCoroutines;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Emitter
{
    public class CrossFadeSoundEmitter : MonoBehaviour
    {
        public bool   playOnStart;
        public string key;

        public float fadeInTime;
        public float fadeOutTime;

        private List<SoundEventInstance> _soundEventInstances;
        private int                      _target = -1;
        private SoundEvent               _soundEvent;

        private void Start()
        {
            _soundEvent          = SoundAccess.GetInstance().RetrieveSoundEvent(key);
            _soundEventInstances = new List<SoundEventInstance>();

            for (int i = 0; i < _soundEvent.audioClips.Count; i++)
            {
                _soundEventInstances.Add(SoundEventInstanceManager.GetInstance().CreateSoundEventInstance(gameObject, _soundEvent));
            }

            foreach (SoundEventInstance soundEventInstance in _soundEventInstances)
            {
                soundEventInstance.SetVolume(0);
            }

            if (playOnStart)
            {
                CrossFadeTo(0, 0);
            }
        }

        public void SetVolume(float volume)
        {
            foreach (SoundEventInstance soundEventInstance in _soundEventInstances)
            {
                soundEventInstance.SetVolume(volume);
            }
        }

        public void CrossFadeTo(Enum targetEnum, float fadeInDurationOverride = -1, float fadeOutDurationOverride = -1)
        {
            int targetIndex = (int)(object)targetEnum;

            CrossFadeTo(targetIndex, fadeInDurationOverride, fadeOutDurationOverride);
        }

        public void CrossFadeTo(int target, float fadeInDurationOverride = -1, float fadeOutDurationOverride = -1)
        {
            StopAllCoroutines();

            if (_target == target)
            {
                return;
            }

            _target = target;

            for (int i = 0; i < _soundEventInstances.Count; i++)
            {
                int iCopy = i;

                void SetVolume(float volume)
                {
                    _soundEventInstances[iCopy].SetVolume(volume);
                }

                if (i == _target)
                {
                    float duration = fadeInDurationOverride != -1 ? fadeInDurationOverride : fadeInTime;

                    _soundEventInstances[i].Play(_soundEvent.audioClips[i]);
                    StartCoroutine(Coroutines.AsynchronousLerpValue(SetVolume, _soundEventInstances[i].GetVolume(), _soundEvent.volume.x, duration));
                }
                else
                {
                    float duration = fadeOutDurationOverride != -1 ? fadeOutDurationOverride : fadeOutTime;

                    _soundEventInstances[i].StopAfter(duration);
                    StartCoroutine(Coroutines.AsynchronousLerpValue(SetVolume, _soundEventInstances[i].GetVolume(), 0, duration));
                }
            }
        }
    }
}