using System.Collections.Generic;
using UnityEngine;

namespace Sound.Event
{
    public class MultiInstrumentSoundEvent: SoundEvent
    {
        public List<AudioClip> audioClips;
    
        public override AudioClip RetrieveAudioClip()
        {
            return audioClips[Random.Range(0, audioClips.Count)];
        }
    }
}