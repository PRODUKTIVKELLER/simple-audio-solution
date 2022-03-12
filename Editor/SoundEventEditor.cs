using System;
using Produktivkeller.SimpleAudioSolution.Event;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Editor.Editor
{
    [CustomEditor(typeof(SoundEvent))]
    public class SoundEventEditor : UnityEditor.Editor
    {
        private float _randomVolume;

        private SoundEvent _soundEvent;


        public void OnEnable()
        {
            _soundEvent = (SoundEvent)target;
        }

        public override void OnInspectorGUI()
        {
            MinMaxSlider("Volume", ref _soundEvent.minVolume, ref _soundEvent.maxVolume);
            MinMaxSlider("Pitch",  ref _soundEvent.minPitch,  ref _soundEvent.maxPitch, -3, 3);

            Checkbox("Spatialize", ref _soundEvent.spatialize);
            FloatSlider("Spatial Blend (2D = 0, 3D = 1)", ref _soundEvent.spatialBlend);
            IntSlider("Max Instances", ref _soundEvent.maxInstances, 1, 1000);
            EnumSelection("Stealing Mode", ref _soundEvent.stealingMode);

            Checkbox("Is Looping", ref _soundEvent.isLooping);
            Checkbox("Reflect",    ref _soundEvent.reflect);

            DrawDefaultInspector();

            if (_soundEvent.spatialBlend == 0)
            {
                return;
            }

            FloatSlider("Doppler Level", ref _soundEvent.dopplerLevel);
            FloatSlider("Spread",        ref _soundEvent.spread);
            MinMaxSlider("Distance", ref _soundEvent.minDistance, ref _soundEvent.maxDistance, 0, 500);
            EnumSelection("Volume Rolloff", ref _soundEvent.volumeRolloff);

            if (_soundEvent.volumeRolloff == VolumeRolloff.Custom)
            {
                AnimationCurve("Rolloff Curve", ref _soundEvent.rolloffCurve);
            }
        }

        private void Checkbox(string label, ref bool value)
        {
            bool oldValue = value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            value = EditorGUILayout.Toggle(value);
            EditorGUILayout.EndHorizontal();

            if (oldValue != value)
            {
                EditorUtility.SetDirty(_soundEvent);
            }
        }

        private void EnumSelection<T>(string label, ref T value) where T : Enum
        {
            T oldValue = value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            value = (T)EditorGUILayout.EnumPopup(value);
            EditorGUILayout.EndHorizontal();

            if (oldValue.CompareTo(value) != 0)
            {
                EditorUtility.SetDirty(_soundEvent);
            }
        }

        private void IntSlider(string label, ref int value, int minLimit = 0, int maxLimit = 1)
        {
            int oldValue = value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            value = EditorGUILayout.IntSlider(value, minLimit, maxLimit);
            EditorGUILayout.EndHorizontal();

            if (oldValue != value)
            {
                EditorUtility.SetDirty(_soundEvent);
            }
        }

        private void FloatSlider(string label, ref float value, float minLimit = 0, float maxLimit = 1)
        {
            float oldValue = value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            value = (float)Math.Round(EditorGUILayout.Slider(value, minLimit, maxLimit), 2);
            EditorGUILayout.EndHorizontal();

            if (oldValue != value)
            {
                EditorUtility.SetDirty(_soundEvent);
            }
        }

        private void MinMaxSlider(string label, ref float min, ref float max, float minLimit = 0,
                                  float  maxLimit = 1)
        {
            float oldMin = min;
            float oldMax = max;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            min = (float)Math.Round(EditorGUILayout.FloatField(min, GUILayout.Width(40.0f)), 2);
            EditorGUILayout.MinMaxSlider(ref min, ref max, minLimit, maxLimit);
            max = (float)Math.Round(EditorGUILayout.FloatField(max, GUILayout.Width(40.0f)), 2);
            EditorGUILayout.EndHorizontal();

            if (oldMin != min || oldMax != max)
            {
                EditorUtility.SetDirty(_soundEvent);
            }
        }

        private void AnimationCurve(string label, ref AnimationCurve animationCurve)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label);
            animationCurve = EditorGUILayout.CurveField(animationCurve);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Mark rolloff as dirty"))
            {
                EditorUtility.SetDirty(_soundEvent);
            }
        }
    }
}