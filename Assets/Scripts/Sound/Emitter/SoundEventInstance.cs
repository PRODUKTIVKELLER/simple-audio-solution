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
            _audioSource.Play();

            DestroyInstanceAfter(_audioSource.clip.length);
        }

        public SoundEvent GetSoundEvent()
        {
            return _soundEvent;
        }

        private void DestroyInstanceAfter(float clipLength)
        {
            Destroy(this, clipLength);
        }

        private void OnDestroy()
        {
            SoundEventInstanceManager.GetInstance().Unregister(this);
            Destroy(_audioSource);
        }
    }
}