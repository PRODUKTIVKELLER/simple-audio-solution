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
            _audioSource.clip = soundEvent.RetrieveAudioClip();

            if (_soundEvent.pitchMin != 1 || _soundEvent.pitchMax != 1)
            {
                _audioSource.pitch = Random.Range(_soundEvent.pitchMin, _soundEvent.pitchMax);
            }

            _audioSource.Play();
        }

        private void Update()
        {
            if (!_audioSource.isPlaying)
            {
                Destroy(this);
            }
        }

        public SoundEvent GetSoundEvent()
        {
            return _soundEvent;
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
    }
}