using System;
using UnityEditor;
using UnityEngine;

namespace Sound.Event.Editor
{
    [CustomEditor(typeof(SoundEvent))]
    public class SoundEventEditor : UnityEditor.Editor
    {
        public float minVolumeVal;
        public float maxVolumeVal;
        public float minPitchVal;
        public float maxPitchVal;

        private float _randomVolume;

        private SoundEvent _soundEvent;
        


        public void OnEnable()
        {
            _soundEvent = (SoundEvent) target;
        }

        public override void OnInspectorGUI()
        {
            MinMaxSlider("Volume", ref _soundEvent.minVolume, ref _soundEvent.maxVolume);
            MinMaxSlider("Pitch", ref _soundEvent.minPitch, ref _soundEvent.maxPitch, -3, 3);

            DrawDefaultInspector();
        }

        private void MinMaxSlider(string label, ref float min, ref float max, float minLimit = 0, float maxLimit = 1)
        {
            float oldMin = min;
            float oldMax = max;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            min = (float) Math.Round(EditorGUILayout.FloatField( min, GUILayout.Width(40.0f)), 2);
            EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
            max = (float) Math.Round(EditorGUILayout.FloatField(max, GUILayout.Width(40.0f)), 2);
            EditorGUILayout.EndHorizontal();

            if (oldMin != min || oldMax != max)
            {
                EditorUtility.SetDirty(_soundEvent);
            }
        }
    }
}