using Sound.Access;
using Sound.Event;
using UnityEngine;

namespace Sound.Emitter
{
    [ExecuteInEditMode]
    public class SoundEmitter : MonoBehaviour
    {
        public string key;

        private SoundEventInstance _soundEventInstance;

        public void Play()
        {
            SoundEvent soundEvent = SoundAccess.GetInstance().RetrieveSoundEvent(key);

            RemoveOldEventInstanceIfKeyHasChanged();
            
            if (!SoundEventInstanceManager.GetInstance().IsAnotherInstanceAllowed(soundEvent))
            {
                return;
            }

            if (_soundEventInstance)
            {
                _soundEventInstance.Play();
                return;
            }

            _soundEventInstance =
                SoundEventInstanceManager.GetInstance().CreateSoundEventInstance(gameObject, soundEvent);

            if (_soundEventInstance != null)
            {
                _soundEventInstance.Play();
            }
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

        // TODO: Put in Editor class.
        private string _oldKey = "";
        [SerializeField] [TextArea(12, 1)] private string autocomplete = "";

        private SoundAccessAutocomplete autocompleter = new SoundAccessAutocomplete();

        private void Update()
        {
            if (key != _oldKey)
            {
                if (autocompleter == null)
                {
                    autocompleter = new SoundAccessAutocomplete();
                }

                autocomplete = autocompleter.GetAutoComplete(key);
            }
        }
    }
}