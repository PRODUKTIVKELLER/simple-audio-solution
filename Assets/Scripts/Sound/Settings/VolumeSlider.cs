using Sound.Access;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Sound.Settings
{
    public class VolumeSlider : MonoBehaviour
    {
        public AudioMixerGroup audioMixerGroup;

        private void Start()
        {
            GetComponent<Slider>().value = SoundAccess.GetInstance().RetrieveVolume(audioMixerGroup.name);
        }

        public void OnValueChange(float valueBetween0And1)
        {
            SoundAccess.GetInstance().ApplyVolume(audioMixerGroup.name, valueBetween0And1);
        }
    }
}