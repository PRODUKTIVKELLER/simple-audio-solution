using System;
using Produktivkeller.SimpleAudioSolution.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Produktivkeller.SimpleAudioSolution.Emitter
{
    public class SoundEventInstance : MonoBehaviour
    {
        private AudioSource _audioSource;
        private bool        _oneShot;
        private SoundEvent  _soundEvent;
        private float       _stopTime;
        private bool        _wasStopped;
        private bool        _isFirstPlayback = true;

        private void Update()
        {
            if (_stopTime != 0 && _stopTime < Time.time)
            {
                Stop();
            }

            bool hasEnded = !_wasStopped && !_audioSource.isPlaying;

            if (hasEnded && _soundEvent.isLooping && _soundEvent.audioClips.Count > 1)
            {
                Play();
            }

            if (_audioSource.isPlaying)
            {
                return;
            }

            if (_oneShot)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            SoundEventInstanceManager.GetInstance().Unregister(this);
            Destroy(_audioSource);
        }

        public void Initialize(SoundEvent soundEvent)
        {
            _soundEvent = soundEvent;

            _audioSource                       = gameObject.AddComponent<AudioSource>();
            _audioSource.loop                  = _soundEvent.isLooping;
            _audioSource.outputAudioMixerGroup = _soundEvent.audioMixerGroup;
            _audioSource.spatialBlend          = _soundEvent.spatialBlend;
            _audioSource.spatialize            = _soundEvent.spatialize;
            _audioSource.priority              = _soundEvent.priority;
            _audioSource.playOnAwake           = false;
            _audioSource.dopplerLevel          = _soundEvent.dopplerLevel;
            _audioSource.spread                = _soundEvent.spread;
            _audioSource.minDistance           = _soundEvent.minDistance;
            _audioSource.maxDistance           = _soundEvent.maxDistance;

            switch (_soundEvent.volumeRolloff)
            {
                case VolumeRolloff.Logarithmic:
                    _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
                    break;
                case VolumeRolloff.Linear:
                    _audioSource.rolloffMode = AudioRolloffMode.Linear;
                    break;
                case VolumeRolloff.Custom:
                    _audioSource.rolloffMode = AudioRolloffMode.Custom;
                    _audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, _soundEvent.rolloffCurve);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SetAndRandomizePitch();
            SetAndRandomizeVolume();
        }

        public SoundEvent GetSoundEvent()
        {
            return _soundEvent;
        }

        private void SetAndRandomizeVolume()
        {
            _audioSource.volume = Random.Range(_soundEvent.minVolume, _soundEvent.maxVolume);
        }

        private void SetAndRandomizePitch()
        {
            _audioSource.pitch = Random.Range(_soundEvent.minPitch, _soundEvent.maxPitch);
        }

        public void Stop()
        {
            _audioSource.Stop();
            _stopTime   = 0;
            _wasStopped = true;
        }

        public void StopAndDestroy()
        {
            Stop();
            Destroy(this);
        }

        public void Play()
        {
            AudioClip audioClip = _soundEvent.RetrieveAudioClip();

            Play(audioClip);
        }

        internal void Play(AudioClip audioClip)
        {
            if (_audioSource.loop && _soundEvent.audioClips.Count > 1)
            {
                _audioSource.loop = false;
            }

            _audioSource.clip = audioClip;

            if (_isFirstPlayback && _soundEvent.ignoreFirstDelay)
            {
                _audioSource.Play();
                _isFirstPlayback = false;
            }
            else
            {
                _audioSource.PlayDelayed(Random.Range(_soundEvent.minDelay, _soundEvent.maxDelay));
            }

            _stopTime   = 0;
            _wasStopped = false;
        }

        public void MarkAsOneShot()
        {
            _oneShot          = true;
            _audioSource.loop = false;
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public float GetVolume()
        {
            return _audioSource.volume;
        }

        public void SetPitch(float pitch)
        {
            _audioSource.pitch = pitch;
        }

        public float GetPitch()
        {
            return _audioSource.pitch;
        }

        public bool IsPlaying()
        {
            return _audioSource.isPlaying;
        }

        public void StopAfter(float waitTime)
        {
            _stopTime = Time.time + waitTime;
        }
    }
}