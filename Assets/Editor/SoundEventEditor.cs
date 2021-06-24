using UnityEditor;
using UnityEngine;

namespace Sound.Event.Editor
{
    [CustomEditor(typeof(SoundEvent), true)]
    public class SoundEventEditor : UnityEditor.Editor
    {
        public float minVolumeVal;
        public float minVolumeLimit = 0;
        public float maxVolumeVal;
        public float maxVolumeLimit = 1;

        public float minPitchVal;
        public float minPitchLimit = 0;
        public float maxPitchVal;
        public float maxPitchLimit = 1;

        private float _randomVolume;

        private bool _randomVolumeActive;

        private SoundEvent _soundEvent;


        public void OnEnable()
        {
            _soundEvent = (SoundEvent) target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Slider("Volume",_soundEvent.volume, 0f, 1f);
            ToggleRandomVolume();

            EditorGUILayout.Slider("Pitch", _soundEvent.volume, -3f, 3f);
            ToggleRandomPitch();

            base.OnInspectorGUI();
        }

        private void ToggleRandomPitch()
        {
            _soundEvent.randomizePitchActive = EditorGUILayout.ToggleLeft("Randomize Pitch", _soundEvent.randomizePitchActive);

            //_soundEvent.randomizePitch = _soundEvent.randomizePitchActive ? EditorGUILayout.Slider(_soundEvent.randomizePitch, -3f, 3f) : 1f;

            if (_soundEvent.randomizePitchActive)
            {
                EditorGUILayout.LabelField("Min Val:", minPitchVal.ToString());
                EditorGUILayout.LabelField("Max Val:", maxPitchVal.ToString());
                EditorGUILayout.MinMaxSlider(ref minPitchVal, ref maxPitchVal, minPitchLimit, maxPitchLimit);
            }
        }

        private void ToggleRandomVolume()
        {
            _randomVolumeActive = EditorGUILayout.ToggleLeft("Randomize Volume", _randomVolumeActive);

            //_randomVolume = _randomVolumeActive ? EditorGUILayout.Slider(_randomVolume, 0f, 1f) : 1f;

            if (_randomVolumeActive)
            {
                EditorGUILayout.LabelField("Min Val:", minVolumeVal.ToString());
                EditorGUILayout.LabelField("Max Val:", maxVolumeVal.ToString());
                EditorGUILayout.MinMaxSlider(ref minVolumeVal, ref maxVolumeVal, minVolumeLimit, maxVolumeLimit);
            }

            _soundEvent.randomizeVolume = _randomVolume;
        }

    }
}