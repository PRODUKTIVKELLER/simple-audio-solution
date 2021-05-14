using System.Collections.Generic;
using Sound.Event;
using UnityEngine;

namespace Sound.Emitter
{
    public class SoundEventInstanceManager
    {
        private readonly Dictionary<string, List<SoundEventInstance>> _soundEventInstances;

        public SoundEventInstance CreateSoundEventInstance(GameObject gameObject, SoundEvent soundEvent)
        {
            SoundEventInstance soundEventInstance = gameObject.AddComponent<SoundEventInstance>();
            soundEventInstance.Initialize(soundEvent);

            if (!_soundEventInstances.ContainsKey(soundEvent.key))
            {
                _soundEventInstances[soundEvent.key] = new List<SoundEventInstance>();
            }

            _soundEventInstances[soundEvent.key].Add(soundEventInstance);
            return soundEventInstance;
        }

        public void Unregister(SoundEventInstance soundEventInstance)
        {
            SoundEvent soundEvent = soundEventInstance.GetSoundEvent();
            _soundEventInstances[soundEvent.key].Remove(soundEventInstance);
        }

        #region Singleton

        private static SoundEventInstanceManager _instance;

        private SoundEventInstanceManager()
        {
            _soundEventInstances = new Dictionary<string, List<SoundEventInstance>>();
        }

        public static SoundEventInstanceManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SoundEventInstanceManager();
            }

            return _instance;
        }

        #endregion
    }
}