using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound.Settings
{
    public abstract class AudioMixerGroupVolume : MonoBehaviour
    {
        public static float RetrieveVolume(AudioMixer audioMixer, String parameterName)
        {
            audioMixer.GetFloat(parameterName, out float value);

            return Mathf.Pow(10f, value / 16f);
        }

        public static void ApplyVolume(AudioMixer audioMixer, String parameterName, float valueBetween0And1)
        {
            audioMixer.SetFloat(parameterName, ValueBetween0And1ToVolume(valueBetween0And1));
        }

        private static float ValueBetween0And1ToVolume(float valueBetween0And1)
        {
            if (valueBetween0And1 == 0)
            {
                valueBetween0And1 = 0.0001f;
            }

            return Mathf.Log10(valueBetween0And1) * 16f;
        }
    }
}