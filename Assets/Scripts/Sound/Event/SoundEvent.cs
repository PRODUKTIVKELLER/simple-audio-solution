using UnityEngine;

namespace Sound.Event
{
    public abstract class SoundEvent : MonoBehaviour
    {
        public abstract AudioClip RetrieveAudioClip();
    }
}