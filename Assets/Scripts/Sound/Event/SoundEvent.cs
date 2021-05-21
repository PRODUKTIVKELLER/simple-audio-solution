using System.Collections.Generic;
using UnityEngine;

namespace Sound.Event
{
    public abstract class SoundEvent : MonoBehaviour
    {
        [HideInInspector] public string key;

        [Header("Random Pitch")] [Range(-3f, 3f)]
        public float pitchMin = 1f;

        [Range(-3f, 3f)] public float pitchMax = 1f;

        [Header("Looping")]
        public bool isLooping = false;

        public abstract AudioClip RetrieveAudioClip();
    }
}