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
        [MinMaxSlider(0, 1)]
        public Vector2 volume = new Vector2(1, 1);
        
        [HideInInspector]
        [MinMaxSlider(-3, 3)]
        public Vector2 pitch = new Vector2(1, 1);
        
        [HideInInspector]
        [MinMaxSlider(0, 30)]
        public Vector2 delay = new Vector2(0, 0);
        
        [HideInInspector]
        [Range(-1, 1)]
        public float stereoPan;
        
        [HideInInspector]
        public string key;
  
        [HideInInspector]
        public bool spatialize;

        [HideInInspector]
        [Range(0, 1)]
        public float spatialBlend = 1f;

        [HideInInspector]
        [Range(1, 1000)]
        public int maxInstances = 1000;

        [HideInInspector]
        public StealingMode stealingMode = StealingMode.Oldest;

        [HideInInspector]
        public bool isLooping;

        [Range(0, 255)]
        public int priority = 128;

        public AudioMixerGroup audioMixerGroup;

        public List<AudioClip> audioClips;
        
        public MultiSoundEventPlaymode multiSoundEventPlaymode = MultiSoundEventPlaymode.Random;
        
        public bool ignoreFirstDelay;

        [Header("3D Settings")]
        public float dopplerLevel;

        [Range(0, 360)]
        public float spread;

        [MinMaxSlider(0, 500)]
        public Vector2 distance = new Vector2(1, 500);
        
        public VolumeRolloff volumeRolloff = VolumeRolloff.Logarithmic;
        
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

            if (_excludedAudioClips == null || _excludedAudioClips.Count >= audioClips.Count)
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