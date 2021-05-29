using Sound.Access;
using Sound.Event;
using UnityEngine;

namespace Sound.Emitter
{
    public class SoundEmitter : MonoBehaviour
    {
        public string key;

        private SoundEventInstance _soundEventInstance;
        

        public void Play()
        {
            SoundEvent soundEvent = SoundAccess.GetInstance().RetrieveSoundEvent(key);

            
            // Das wollen wir machen falls der Key sich geändert hat.
            if (_soundEventInstance)
            {
                _soundEventInstance.Stop();
            }

            _soundEventInstance = SoundEventInstanceManager.GetInstance().CreateSoundEventInstance(gameObject, soundEvent);
        }

        public void Stop()
        {
            if (!_soundEventInstance)
            {
                return;
            }
            
            // Das hier sollte den Sound nur stoppen, nicht die Instanz zerstören.
            _soundEventInstance.Stop();
        }
    }
}