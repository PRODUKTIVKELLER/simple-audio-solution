using System;
using System.Collections.Generic;
using System.Linq;
using Produktivkeller.SimpleAudioSolution.Access;
using Produktivkeller.SimpleAudioSolution.Event;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Emitter
{
    public class SoundEventInstanceManager
    {
        private readonly Dictionary<string, List<SoundEventInstance>> _soundEventInstances;
        private          Dictionary<SoundEventInstance, float>        _mutedLoopingSoundEventInstances;

        private List<SoundEventInstance> GetInstances(string key)
        {
            if (!_soundEventInstances.ContainsKey(key))
            {
                return new List<SoundEventInstance>();
            }

            return _soundEventInstances[key];
        }

        private List<SoundEventInstance> RetrieveCurrentlyLoopingSoundEventInstances()
        {
            List<SoundEvent>         loopingSoundEvents         = SoundAccess.GetInstance().RetrieveLoopingSoundEvents();
            List<SoundEventInstance> currentlyLoopingSoundEventInstances = new List<SoundEventInstance>();

            foreach (SoundEvent loopingSoundEvent in loopingSoundEvents)
            {
                if (!_soundEventInstances.TryGetValue(loopingSoundEvent.key, out List<SoundEventInstance> loopingSoundEventInstances))
                {
                    continue;
                }

                loopingSoundEventInstances = loopingSoundEventInstances.Where(s => s.IsPlaying()).ToList();
                    
                currentlyLoopingSoundEventInstances.AddRange(loopingSoundEventInstances);
            }

            return currentlyLoopingSoundEventInstances;
        }
        
        public void MuteLoopingSoundEventInstances()
        {
            List<SoundEventInstance> currentlyLoopingSoundEventInstances = RetrieveCurrentlyLoopingSoundEventInstances();

            foreach (SoundEventInstance currentlyLoopingSoundEventInstance in currentlyLoopingSoundEventInstances)
            {
                _mutedLoopingSoundEventInstances.Add(currentlyLoopingSoundEventInstance, currentlyLoopingSoundEventInstance.GetVolume());
                
                currentlyLoopingSoundEventInstance.SetVolume(0);
            }
        }

        public void UnmuteLoopingSoundEventInstances()
        {
            foreach (KeyValuePair<SoundEventInstance, float> entry in _mutedLoopingSoundEventInstances)
            {
                entry.Key.SetVolume(entry.Value);
            }
            
            _mutedLoopingSoundEventInstances = new Dictionary<SoundEventInstance, float>();
        }

        public SoundEventInstance CreateSoundEventInstance(GameObject gameObject, SoundEvent soundEvent)
        {
            SoundEventInstance soundEventInstance = gameObject.AddComponent<SoundEventInstance>();
            soundEventInstance.Initialize(soundEvent);

            if (!_soundEventInstances.ContainsKey(soundEvent.key))
            {
                _soundEventInstances[soundEvent.key] = new List<SoundEventInstance>();
            }

            RemoveOldInstanceIfApplicable(soundEvent);

            _soundEventInstances[soundEvent.key].Add(soundEventInstance);
            return soundEventInstance;
        }

        public SoundEventInstance CreateOneShotSoundEventInstance(SoundEvent soundEvent, Vector3 position)
        {
            if (!IsAllowedToPlay(soundEvent))
            {
                return null;
            }

            GameObject oneShotGameObject = new GameObject("One Shot - " + soundEvent.key);
            oneShotGameObject.transform.position = position;

            SoundEventInstance soundEventInstance = CreateSoundEventInstance(oneShotGameObject, soundEvent);
            soundEventInstance.MarkAsOneShot();

            return soundEventInstance;
        }

        private void RemoveOldInstanceIfApplicable(SoundEvent soundEvent)
        {
            List<SoundEventInstance> instances = GetInstances(soundEvent.key);

            if (instances.Count < soundEvent.maxInstances)
            {
                return;
            }

            if (soundEvent.stealingMode == StealingMode.Oldest)
            {
                instances[0].StopAndDestroy();
                instances.RemoveAt(0);
            }
            else if (soundEvent.stealingMode == StealingMode.Newest)
            {
                instances[instances.Count - 1].StopAndDestroy();
                instances.RemoveAt(instances.Count - 1);
            }
        }

        public bool IsAllowedToPlay(SoundEvent soundEvent)
        {
            if (soundEvent.stealingMode != StealingMode.None)
            {
                return true;
            }

            List<SoundEventInstance> instances = GetInstances(soundEvent.key);
            
            return instances.Count(i => i.IsPlaying()) < soundEvent.maxInstances;
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
            _soundEventInstances             = new Dictionary<string, List<SoundEventInstance>>();
            _mutedLoopingSoundEventInstances = new Dictionary<SoundEventInstance, float>();
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