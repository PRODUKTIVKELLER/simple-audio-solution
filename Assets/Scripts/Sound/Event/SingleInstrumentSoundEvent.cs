using UnityEngine;

namespace Sound.Event
{
    public class SingleInstrumentSoundEvent: SoundEvent
    {
        public AudioClip audioClip;
    
        public override AudioClip RetrieveAudioClip()
        {
            return audioClip;
        }
    }
}