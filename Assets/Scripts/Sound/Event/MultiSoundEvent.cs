using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sound.Event
{
    public class MultiSoundEvent : SoundEvent
    {
        [Header("Audio Clips")]
        public List<AudioClip> audioClips;

        [Header("Multi Sound Event Playmode")]
        public MultiSoundEventPlaymode multiSoundEventPlaymode = MultiSoundEventPlaymode.Random;

        private List<AudioClip> _previouslyPlayedAudioClips = new List<AudioClip>(); 
        
        public void Start()
        {
            if (audioClips.Count == 0)
            {
                Debug.LogError("'SoundEvent' [" + key + "] without audio clips is not allowed.");
            }
        }

        private AudioClip PickRandomClip(List<AudioClip> clips)
        {
            return clips[Random.Range(0, clips.Count)];  
        }
        
        public override AudioClip RetrieveAudioClip()
        {
            if (multiSoundEventPlaymode == MultiSoundEventPlaymode.Random)
            {
                return PickRandomClip(audioClips);
            }

            if (_previouslyPlayedAudioClips.Count >= audioClips.Count)
            {
                _previouslyPlayedAudioClips = new List<AudioClip>(); 
            }
                
            List<AudioClip> audioClipsForShuffle = audioClips.Except(_previouslyPlayedAudioClips).ToList();
            AudioClip shuffledAudioClip = PickRandomClip(audioClipsForShuffle);
                
            _previouslyPlayedAudioClips.Add(shuffledAudioClip);

            return shuffledAudioClip;
        }
    }
}