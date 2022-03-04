using Produktivkeller.SimpleAudioSolution.Access;
using Produktivkeller.SimpleAudioSolution.Event;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Emitter
{
    public class SoundEmitter : MonoBehaviour
    {
        public bool   playOnStart;
        public string key;

        private SoundEventInstance _soundEventInstance;

        private void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        public void Play()
        {
            SoundEvent soundEvent = SoundAccess.GetInstance().RetrieveSoundEvent(key);

            RemoveOldEventInstanceIfKeyHasChanged();

            if (_soundEventInstance)
            {
                _soundEventInstance.Play();
                return;
            }

            if (!SoundEventInstanceManager.GetInstance().IsAnotherInstanceAllowed(soundEvent))
            {
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

        public void SetVolume(float volume)
        {
            if (!_soundEventInstance)
            {
                return;
            }

            _soundEventInstance.SetVolume(volume);
        }

        public float GetVolume()
        {
            if (!_soundEventInstance)
            {
                return -1f;
            }

            return _soundEventInstance.GetVolume();
        }

        public bool IsPlaying()
        {
            if (!_soundEventInstance)
            {
                return false;
            }

            return _soundEventInstance.IsPlaying();
        }

        // // TODO: Put in Editor class.
        // private string _oldKey = "";
        // [SerializeField] [TextArea(12, 1)] private string autocomplete = "";
        //
        // private SoundAccessAutocomplete autocompleter = new SoundAccessAutocomplete();
        //
        // private void Update()
        // {
        //     if (key != _oldKey)
        //     {
        //         if (autocompleter == null)
        //         {
        //             autocompleter = new SoundAccessAutocomplete();
        //         }
        //
        //         autocomplete = autocompleter.GetAutoComplete(key);
        //     }
        // }
    }
}