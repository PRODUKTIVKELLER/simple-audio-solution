using System.Collections.Generic;
using Sound.Event;
using UnityEngine;

namespace Sound.Access
{
    public class SoundAccess : MonoBehaviour
    {
        private Dictionary<string, SoundEvent> _soundEvents;

        private void Awake()
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

            return _soundEvents[id];
        }
    }
}