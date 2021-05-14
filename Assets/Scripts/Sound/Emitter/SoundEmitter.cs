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
            
            _soundEventInstance.Stop();
        }
    }
}