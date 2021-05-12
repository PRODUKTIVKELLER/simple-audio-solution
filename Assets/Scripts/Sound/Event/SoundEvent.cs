using UnityEngine;

namespace Sound.Event
{
    public abstract class SoundEvent : MonoBehaviour
    {
        [HideInInspector]
        public string key;
        
        public abstract AudioClip RetrieveAudioClip();
    }
}