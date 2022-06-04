using Produktivkeller.SimpleAudioSolution.Access;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Produktivkeller.SimpleAudioSolution.Settings
{
    public class VolumeSlider : MonoBehaviour
    {
        public  AudioMixerGroup audioMixerGroup;
        private string          _persistenceKey;

        private void Start()
        {
            _persistenceKey = "volume." + audioMixerGroup.name;

            if (!PlayerPrefs.HasKey(_persistenceKey))
            {
                float currentVolume = SoundAccess.GetInstance().RetrieveVolume(audioMixerGroup.name);
                SaveVolume(currentVolume);
            }

            GetComponent<Slider>().value = PlayerPrefs.GetFloat(_persistenceKey);
        }

        public void OnValueChange(float valueBetween0And1)
        {
            SaveVolume(valueBetween0And1);
            SoundAccess.GetInstance().ApplyVolume(audioMixerGroup.name, valueBetween0And1);
        }

        private void SaveVolume(float value)
        {
            PlayerPrefs.SetFloat(_persistenceKey, value);
            PlayerPrefs.Save();
        }
    }
}