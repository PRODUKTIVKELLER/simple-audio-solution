using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Produktivkeller.SimpleAudioSolution.Emitter;
using Produktivkeller.SimpleAudioSolution.Event;
using Produktivkeller.SimpleAudioSolution.Settings;
using UnityEngine;
using UnityEngine.Audio;

namespace Produktivkeller.SimpleAudioSolution.Access
{
    public class SoundAccess : MonoBehaviour
    {
        private AudioMixer                     _audioMixer;
        private Dictionary<string, SoundEvent> _soundEvents;

        private const string PERSISTENCE_KEY_VOLUME = "volume.";

        private void Initialize()
        {
            LoadAudioMixer();
            LoadSoundEvents();
        }

        private void Start()
        {
            LoadVolume();
        }

        private void LoadAudioMixer()
        {
            AudioMixer[] audioMixers = Resources.LoadAll<AudioMixer>("");

            if (audioMixers.Length == 0)
            {
                Debug.LogError("No AudioMixer was found.");
                return;
            }
            
            _audioMixer = audioMixers[0];
        }

        private void LoadSoundEvents()
        {
            _soundEvents = new Dictionary<string, SoundEvent>();

            LoadSoundEventsRecursively(transform, "", null);
        }

        private void LoadVolume()
        {
            List<AudioMixerGroup> audioMixerGroups = _audioMixer
                                                     .FindMatchingGroups(string.Empty)
                                                     .Where(a => !a.name.Contains("Intern") && a.name != "Reverb")
                                                     .ToList();

            foreach (AudioMixerGroup audioMixerGroup in audioMixerGroups)
            {
                string persistenceKey = PERSISTENCE_KEY_VOLUME + audioMixerGroup.name;

                if (!PlayerPrefs.HasKey(persistenceKey))
                {
                    PlayerPrefs.SetFloat(persistenceKey, AudioMixerGroupVolume.RetrieveVolume(_audioMixer, audioMixerGroup.name));
                    PlayerPrefs.Save();

                    continue;
                }

                ApplyVolume(audioMixerGroup.name, PlayerPrefs.GetFloat(persistenceKey));
            }
        }

        private void LoadSoundEventsRecursively(Transform current, string path, AudioMixerGroup audioMixerGroup)
        {
            bool isRoot = current.GetComponent<SoundAccess>() != null;

            if (!isRoot && !audioMixerGroup)
            {
                AudioMixerGroup[] audioMixerGroups = _audioMixer.FindMatchingGroups("Intern - " + current.name);

                if (audioMixerGroups.Length == 0)
                {
                    Debug.LogError("No AudioMixerGroup named '" + current.name + "' was found.");
                }
                else
                {
                    audioMixerGroup = audioMixerGroups[0];
                }
            }

            if (!isRoot)
            {
                path += "/" + current.name;

                SoundEvent soundEvent = current.GetComponent<SoundEvent>();

                if (soundEvent)
                {
                    _soundEvents[path] = soundEvent;

                    if (!soundEvent.audioMixerGroup)
                    {
                        soundEvent.audioMixerGroup = audioMixerGroup;
                    }

                    return;
                }
            }

            foreach (Transform child in current)
            {
                LoadSoundEventsRecursively(child, path, audioMixerGroup);
            }
        }

        private string SoundEventToPath(SoundEvent soundEvent, Transform current, string path)
        {
            bool isRoot = current.GetComponent<SoundAccess>() != null;
            
            if (!isRoot)
            {
                path += "/" + current.name;

                SoundEvent currentSoundEvent = current.GetComponent<SoundEvent>();
                
                if (currentSoundEvent)
                {
                    if (currentSoundEvent == soundEvent)
                    {
                        return path;
                    }

                    return null;
                }
            }

            foreach (Transform child in current)
            {
                string childPath = SoundEventToPath(soundEvent, child, path);

                if (childPath != null)
                {
                    return childPath;
                }
            }

            return null;
        }

        public SoundEvent RetrieveSoundEvent(string key)
        {
            if (!_soundEvents.ContainsKey(key))
            {
                Debug.LogError("SoundEvent ID '" + key + "' not found.");
                return null;
            }

            SoundEvent soundEvent = _soundEvents[key];
            soundEvent.key = key;

            return soundEvent;
        }

        public float RetrieveVolume(String parameter)
        {
            string persistenceKey = PERSISTENCE_KEY_VOLUME + parameter;

            if (PlayerPrefs.HasKey(persistenceKey))
            {
                return PlayerPrefs.GetFloat(persistenceKey);
            }

            float volume = AudioMixerGroupVolume.RetrieveVolume(_audioMixer, parameter);
            PlayerPrefs.SetFloat(persistenceKey, volume);
            PlayerPrefs.Save();

            return volume;
        }

        public void ApplyVolume(String parameter, float valueBetween0And1)
        {
            string persistenceKey = PERSISTENCE_KEY_VOLUME + parameter;

            PlayerPrefs.SetFloat(persistenceKey, valueBetween0And1);
            PlayerPrefs.Save();

            AudioMixerGroupVolume.ApplyVolume(_audioMixer, parameter, valueBetween0And1);
        }

        public SoundEventInstance PlayOneShot(string soundEventKey, Vector3 position)
        {
            SoundEventInstance soundEventInstance = SoundEventInstanceManager.GetInstance()
                                                                             .CreateOneShotSoundEventInstance(RetrieveSoundEvent(soundEventKey), position);

            if (soundEventInstance)
            {
                soundEventInstance.Play();
            }

            return soundEventInstance;
        }

        public SoundEventInstance PlayOneShot2D(string soundEventKey)
        {
            return PlayOneShot(soundEventKey, Vector3.zero);
        }

        public void PlayOneShot2DDelayed(string soundEventKey, float delay)
        {
            StartCoroutine(PlayOneShot2DDelayedAsync(soundEventKey, delay));
        }

        private IEnumerator PlayOneShot2DDelayedAsync(string soundEventKey, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlayOneShot2D(soundEventKey);
        }

        public void SoundEventHasChangedInPlayMode(SoundEvent soundEvent)
        {
            string key = SoundEventToPath(soundEvent, transform, "");
            SoundEventInstanceManager.GetInstance().UpdateSoundEventInstancesInPlayMode(key);
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
                transform.SetParent(null);
                DontDestroyOnLoad(this);
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