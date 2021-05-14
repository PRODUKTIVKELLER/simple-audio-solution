using System.Collections.Generic;
using UnityEngine;

namespace Sound.Event
{
    public class SoundEvent : MonoBehaviour
    {
        [HideInInspector] public string key;

        public List<AudioClip> audioClips;

        [Header("Random Pitch")] [Range(-3f, 3f)]
        public float pitchMin = 1f;

        [Range(-3f, 3f)] public float pitchMax = 1f;
        
        public void Start()
        {
            if (audioClips.Count == 0)
            {
                Debug.LogError("'SoundEvent' [" + key + "] without audio clips is not allowed.");
            }
        }

        public AudioClip RetrieveAudioClip()
        {
            return audioClips[Random.Range(0, audioClips.Count)];
        }
    }
}