using System;
using System.Collections;
using System.Collections.Generic;
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

        private void Initialize()
        {
            LoadAudioMixer();
            LoadSoundEvents();
        }

        private void LoadAudioMixer()
        {
            _audioMixer = Resources.Load("Audio Mixer") as AudioMixer;

            if (!_audioMixer)
            {
                Debug.LogError("No AudioMixer was found.");
            }
        }

        private void LoadSoundEvents()
        {
            _soundEvents = new Dictionary<string, SoundEvent>();

            LoadSoundEventsRecursively(transform, "", null);
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

        public SoundEvent RetrieveSoundEvent(string key)
        {
            if (!_soundEvents.ContainsKey(key))
            {
                Debug.LogError("SoundEvent ID '" + key + "' not found.");
            }

            SoundEvent soundEvent = _soundEvents[key];
            soundEvent.key = key;

            return soundEvent;
        }

        public float RetrieveVolume(String parameter)
        {
            return AudioMixerGroupVolume.RetrieveVolume(_audioMixer, parameter);
        }

        public void ApplyVolume(String parameter, float valueBetween0And1)
        {
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