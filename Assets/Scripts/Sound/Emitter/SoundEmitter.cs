using System.Collections.Generic;
using Sound.Access;
using Sound.Event;
using UnityEngine;
using UnityEngine.Events;

namespace Sound.Emitter
{
    [ExecuteInEditMode]
    public class SoundEmitter : MonoBehaviour
    {
        public string key;

        private SoundEventInstance _soundEventInstance;
        private bool _maxInstancesAllowAnotherInstance = true;
        
        public void Play()
        {
            SoundEvent soundEvent = SoundAccess.GetInstance().RetrieveSoundEvent(key);

            RemoveOldEventInstanceIfKeyHasChanged();
            
            if (_soundEventInstance)
            {
                _soundEventInstance.Play();
                return;
            }

            HandleMaxInstances(soundEvent);

            if (_maxInstancesAllowAnotherInstance)
            {
                _soundEventInstance =
                    SoundEventInstanceManager.GetInstance().CreateSoundEventInstance(gameObject, soundEvent);

                if (_soundEventInstance != null)
                {
                    _soundEventInstance.Play();
                }
            }

            _maxInstancesAllowAnotherInstance = true;
        }

        private void HandleMaxInstances(SoundEvent soundEvent)
        {
            List<SoundEventInstance> instances = SoundEventInstanceManager.GetInstance().GetInstances(key);
            int instancesCount = instances.Count;

            if (instancesCount >= soundEvent.maxInstances)
            {
                if (soundEvent.stealingMode == StealingMode.Oldest)
                {
                    instances[0].StopAndDestroy();
                }
                else if (soundEvent.stealingMode == StealingMode.Newest)
                {
                    instances[instancesCount - 1].StopAndDestroy();
                }
                else
                {
                    _maxInstancesAllowAnotherInstance = false;
                }
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


        private string _oldKey = "";
        [SerializeField] [TextArea(12, 1)]
        private string autocomplete = "";

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