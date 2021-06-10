using System.Collections.Generic;
using System.Linq;
using Sound.Emitter;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound.Event
{
    public class SoundEvent : MonoBehaviour
    {
        [HideInInspector] public string key;

        [Header("Volume")] [Range(0f, 1f)] public float volume = 1f;
        [HideInInspector] public float randomizeVolume = 1f;

        [Header("Pitch")] [Range(-3f, 3f)] public float pitch = 1f;
        
        [Header("Max Instances")] [Range(1, 1000)] public int maxInstances = 1000;
        
        [Header("Stealing Mode")] public StealingMode stealingMode = StealingMode.Oldest;

        [HideInInspector] public bool randomizePitchActive;

        [HideInInspector] public float randomizePitch = 1f;
        
        [Header("Looping")] public bool isLooping;

        [Header("Audio Clips")] public List<AudioClip> audioClips;

        [Header("Multi Sound Event Playmode")]
        public MultiSoundEventPlaymode multiSoundEventPlaymode = MultiSoundEventPlaymode.Random;

        [Header("Audio Mixer Group")]
        public AudioMixerGroup audioMixerGroup;

        private List<AudioClip> _previouslyPlayedAudioClips;

        public void Start()
        {
            _previouslyPlayedAudioClips = new List<AudioClip>();

            if (audioClips == null || audioClips.Count == 0)
            {
                Debug.LogError("'SoundEvent' [" + key + "] without audio clips is not allowed.");
            }
        }

        public AudioClip RetrieveAudioClip()
        {
            if (audioClips.Count == 1)
            {
                return audioClips[0];
            }

            return RetrieveAudioClipForMultiInstrument();
        }

        private AudioClip RetrieveAudioClipForMultiInstrument()
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

        private static AudioClip PickRandomClip(IReadOnlyList<AudioClip> clips)
        {
            return clips[Random.Range(0, clips.Count)];
        }
    }
}