using System.Collections.Generic;
using System.Linq;
using Produktivkeller.SimpleAudioSolution.Emitter;
using UnityEngine;
using UnityEngine.Audio;

namespace Produktivkeller.SimpleAudioSolution.Event
{
    public class SoundEvent : MonoBehaviour
    {
        [HideInInspector]
        public string key;

        [HideInInspector]
        public float minVolume = 1f;

        [HideInInspector]
        public float maxVolume = 1f;

        [HideInInspector]
        public float minPitch = 1f;

        [HideInInspector]
        public float maxPitch = 1f;

        [HideInInspector]
        public bool spatialize;

        [HideInInspector]
        public float spatialBlend = 1f;

        [HideInInspector]
        public int maxInstances = 1000;

        [HideInInspector]
        public StealingMode stealingMode = StealingMode.Oldest;

        [HideInInspector]
        public bool isLooping;

        [HideInInspector]
        public bool reflect;

        public int priority = 128;

        [Header("Audio Clips")]
        public List<AudioClip> audioClips;

        [Header("Multi Sound Event Playmode")]
        public MultiSoundEventPlaymode multiSoundEventPlaymode = MultiSoundEventPlaymode.Random;

        [Header("Audio Mixer Group")]
        public AudioMixerGroup audioMixerGroup;

        [HideInInspector]
        public float dopplerLevel;

        [HideInInspector]
        public float spread;

        [HideInInspector]
        public float minDistance = 1f;

        [HideInInspector]
        public float maxDistance = 500f;

        [HideInInspector]
        public VolumeRolloff volumeRolloff = VolumeRolloff.Logarithmic;

        [HideInInspector]
        public AnimationCurve rolloffCurve;

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
            AudioClip       shuffledAudioClip    = PickRandomClip(audioClipsForShuffle);

            _previouslyPlayedAudioClips.Add(shuffledAudioClip);

            return shuffledAudioClip;
        }

        private static AudioClip PickRandomClip(IReadOnlyList<AudioClip> clips)
        {
            return clips[Random.Range(0, clips.Count)];
        }
    }
}