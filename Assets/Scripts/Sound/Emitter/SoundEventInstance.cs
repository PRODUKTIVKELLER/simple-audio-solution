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

            SetAndRandomizePitch();
            SetAndRandomizeVolume();

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

            // Nur wegwerfen, wenn die Instanz nicht mehr von einem Emitter gebraucht wird.
            
            // This might not be optimal, since when the sound should still fade out after being stopped,
            // this SoundEventInstance should not be destroyed immediately
            Destroy(this);
        }
    }
}