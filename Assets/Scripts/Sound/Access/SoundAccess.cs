using System.Collections.Generic;
using Sound.Event;
using UnityEngine;

namespace Sound.Access
{
    public class SoundAccess : MonoBehaviour
    {
        private Dictionary<string, SoundEvent> _soundEvents;
        
        private void Initialize()
        {
            LoadSoundEvents();
        }
        
        private void LoadSoundEvents()
        {
            _soundEvents = new Dictionary<string, SoundEvent>();

            LoadSoundEventsRecursively(transform, "");
        }

        private void LoadSoundEventsRecursively(Transform current, string path)
        {
            bool isRoot = current.GetComponent<SoundAccess>() != null;

            if (!isRoot)
            {
                path += "/" + current.name;

                SoundEvent soundEvent = current.GetComponent<SoundEvent>();

                if (soundEvent)
                {
                    _soundEvents[path] = soundEvent;
                    return;
                }
            }

            foreach (Transform child in current)
            {
                LoadSoundEventsRecursively(child, path);
            }
        }

        public SoundEvent RetrieveSoundEvent(string id)
        {
            if (!_soundEvents.ContainsKey(id))
            {
                Debug.LogError("SoundEvent ID '" + id + "' not found.");
            }

            SoundEvent soundEvent = _soundEvents[id];
            soundEvent.key = id;

            return soundEvent;
        }
        
        
        #region Singleton

        private static SoundAccess _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else if (_instance == null)
            {
                _instance = this;
                Initialize();
            }
        }
        
        public static SoundAccess GetInstance()
        {
            return _instance;
        }
        
        #endregion
    }
}