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

        [Header("Audio Mixer Group")]
        public AudioMixerGroup audioMixerGroup;

        [Header("Audio Clips")]
        public List<AudioClip> audioClips;

        [HideInInspector]
        public MultiSoundEventPlaymode multiSoundEventPlaymode = MultiSoundEventPlaymode.Random;

        [HideInInspector]
        public float minDelay = 0f;

        [HideInInspector]
        public float maxDelay = 0f;

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

        private List<AudioClip> _excludedAudioClips;
        private AudioClip       _previousAudioClip;

        public void Start()
        {
            _excludedAudioClips = new List<AudioClip>();

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

            if (_excludedAudioClips.Count >= audioClips.Count)
            {
                _excludedAudioClips = new List<AudioClip>();

                if (_previousAudioClip != null)
                {
                    _excludedAudioClips.Add(_previousAudioClip);
                }
            }

            List<AudioClip> audioClipsForShuffle = audioClips.Except(_excludedAudioClips).ToList();
            AudioClip       shuffledAudioClip    = PickRandomClip(audioClipsForShuffle);

            _previousAudioClip = shuffledAudioClip;
            _excludedAudioClips.Add(shuffledAudioClip);

            return shuffledAudioClip;
        }

        private static AudioClip PickRandomClip(IReadOnlyList<AudioClip> clips)
        {
            return clips[Random.Range(0, clips.Count)];
        }
    }
}