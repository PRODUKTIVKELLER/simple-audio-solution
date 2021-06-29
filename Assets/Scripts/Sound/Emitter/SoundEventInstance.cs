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
            _audioSource.spatialBlend = _soundEvent.spatialBlend;

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

        private void SetAndRandomizeVolume()
        {
            _audioSource.volume = Random.Range(_soundEvent.minVolume, _soundEvent.maxVolume);
        }

        private void SetAndRandomizePitch()
        {
            _audioSource.pitch = Random.Range(_soundEvent.minPitch, _soundEvent.maxPitch);
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