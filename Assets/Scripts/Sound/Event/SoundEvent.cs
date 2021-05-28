using UnityEngine;

namespace Sound.Event
{
    public abstract class SoundEvent : MonoBehaviour
    {
        [HideInInspector] public string key;

        [Header("Volume")]
        [Range(0f, 1f)]
        public float volume = 1f;
        [HideInInspector] public float randomizeVolume = 1f;


        [Header("Pitch")]
        [Range(-3f, 3f)]
        public float pitch = 1f;

        [HideInInspector] public float randomizePitch = 1f;


        [Header("Looping")]
        public bool isLooping = false;

        public abstract AudioClip RetrieveAudioClip();
    }
}