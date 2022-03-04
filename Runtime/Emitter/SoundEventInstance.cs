using Produktivkeller.SimpleAudioSolution.Event;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Emitter
{
    public class SoundEventInstance : MonoBehaviour
    {
        private AudioSource _audioSource;
        private bool        _oneShot;
        private SoundEvent  _soundEvent;
        private float       _stopTime;

        private void Update()
        {
            if (_stopTime != 0 && _stopTime < Time.time)
            {
                Stop();
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
            _stopTime = 0;
        }

        public void StopAndDestroy()
        {
            Stop();
            Destroy(this);
        }

        public void Play()
        {
            _audioSource.clip = _soundEvent.RetrieveAudioClip();

            _audioSource.Play();
            _stopTime = 0;
        }

        internal void Play(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;

            _audioSource.Play();
            _stopTime = 0;
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