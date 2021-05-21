using UnityEngine;

namespace Sound.Event
{
    public class SingleSoundEvent : SoundEvent
    {
        [Header("Audio Clip")]
        public AudioClip audioClip;
        
        public void Start()
        {
            if (audioClip == null)
            {
                Debug.LogError("'SoundEvent' [" + key + "] without audio clips is not allowed.");
            }
        }
        
        public override AudioClip RetrieveAudioClip()
        {
            return audioClip;
        }
    }
}