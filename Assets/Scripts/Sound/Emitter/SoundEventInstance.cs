using Sound.Event;
using UnityEngine;

namespace Sound.Emitter
{
    public class SoundEventInstance : MonoBehaviour
    {
        private AudioSource _audioSource;
        private SoundEvent _soundEvent;

        public void Initialize(SoundEvent soundEvent)
        {
            _soundEvent = soundEvent;

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = _soundEvent.isLooping;
            _audioSource.outputAudioMixerGroup = _soundEvent.audioMixerGroup;
            
            SetAndRandomizePitch();
            SetAndRandomizeVolume();
        }

        private void Update()
        {
            if (_audioSource.isPlaying)
            {
                return;
            }

            Destroy(this);
        }

        public SoundEvent GetSoundEvent()
        {
            return _soundEvent;
        }

        public void SetAndRandomizeVolume()
        {
            if (_soundEvent.volume != 1)
            {
                _audioSource.volume = _soundEvent.volume;
            }

            if (_soundEvent.randomizeVolume != 1)
            {
                _audioSource.volume = Random.Range(_soundEvent.randomizeVolume, _soundEvent.volume);
            }
        }

        public void SetAndRandomizePitch()
        {
            if (_soundEvent.pitch != 1)
            {
                _audioSource.pitch = _soundEvent.pitch;
            }

            if (_soundEvent.randomizePitch != 1)
            {
                _audioSource.pitch = Random.Range(_soundEvent.randomizePitch, _soundEvent.pitch);
            }
        }

        private void OnDestroy()
        {
            SoundEventInstanceManager.GetInstance().Unregister(this);
            Destroy(_audioSource);
        }

        public void Stop()
        {
            _audioSource.Stop();
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
        }
    }
}