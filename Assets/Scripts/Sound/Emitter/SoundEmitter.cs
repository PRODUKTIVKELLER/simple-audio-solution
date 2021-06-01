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

            RemoveOldEventInstanceIfKeyHasChanged();
            
            if (_soundEventInstance)
            {
                _soundEventInstance.Play();
                return;
            }

            _soundEventInstance =
                SoundEventInstanceManager.GetInstance().CreateSoundEventInstance(gameObject, soundEvent);
            _soundEventInstance.Play();
        }

        private void RemoveOldEventInstanceIfKeyHasChanged()
        {
            if (!_soundEventInstance || _soundEventInstance.GetSoundEvent().key == key)
            {
                return;
            }
            
            _soundEventInstance.StopAndDestroy();
            _soundEventInstance = null;
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