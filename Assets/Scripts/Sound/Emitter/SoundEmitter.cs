using Sound.Access;
using Sound.Event;
using UnityEngine;

namespace Sound.Emitter
{
    public class SoundEmitter : MonoBehaviour
    {
        public string key;

        private void Start()
        {
            Play();
        }

        public void Play()
        {
            SoundEvent soundEvent = SoundAccess.GetInstance().RetrieveSoundEvent(key);
            
            SoundEventInstanceManager.GetInstance().CreateSoundEventInstance(gameObject, soundEvent);
        }
    }
}