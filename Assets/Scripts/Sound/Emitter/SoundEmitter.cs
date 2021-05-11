using Sound.Access;
using Sound.Event;
using UnityEngine;

namespace Sound.Emitter
{
    public class SoundEmitter : MonoBehaviour
    {
        public SoundAccess soundAccess;

        public string key;

        private void Start()
        {
            Play();
        }

        public void Play()
        {
            SoundEvent soundEvent = soundAccess.RetrieveSoundEvent(key);
        
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = soundEvent.RetrieveAudioClip();
            audioSource.Play();

            // TODO
            // audioSource.pitch = soundEvent.DetermineRandomPitch();
        }
    }
}