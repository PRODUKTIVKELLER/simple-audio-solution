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

            // To catch creating multiple AudioSources, this is being checked here.
            // When playing a looping sound, this Initialize() function is called more than once.
            if (!_audioSource)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
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
                if (!_soundEvent.isLooping)
                {
                    Destroy(this);
                }
                else
                {
                    Initialize(_soundEvent);
                }
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

            // This might not be optimal, since when the sound should still fade out after being stopped,
            // this SoundEventInstance should not be destroyed immediately
            Destroy(this);
        }
    }
}