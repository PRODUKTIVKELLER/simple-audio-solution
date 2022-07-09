using Produktivkeller.SimpleAudioSolution.Event;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Editor.Editor
{
    [CustomEditor(typeof(SoundEvent))]
    public class SoundEventEditor : UnityEditor.Editor
    {
        private float _randomVolume;

        private SoundEvent       _soundEvent;
        private SerializedObject _serializedObject;

        public void OnEnable()
        {
            _soundEvent = (SoundEvent)target;
        }

        public override void OnInspectorGUI()
        {
            _serializedObject = new SerializedObject(_soundEvent);

            PropertyField("volume");
            PropertyField("pitch");
            PropertyField("delay");
            PropertyField("spatialize");
            PropertyField("spatialBlend", "Spatial Blend (2D = 0, 3D = 1)");
            PropertyField("maxInstances");
            PropertyField("stealingMode");
            PropertyField("isLooping");
            PropertyField("priority", "Priority (0 = High Priority)");
            PropertyField("audioMixerGroup");
            PropertyField("audioClips");

            if (_soundEvent.audioClips.Count > 1)
            {
                PropertyField("multiSoundEventPlaymode", "Playmode");

                if (_soundEvent.isLooping)
                {
                    PropertyField("ignoreFirstDelay");
                }
            }

            if (_soundEvent.spatialBlend != 0)
            {
                PropertyField("dopplerLevel");
                PropertyField("spread");
                PropertyField("distance");
                PropertyField("volumeRolloff");

                if (_soundEvent.volumeRolloff == VolumeRolloff.Custom)
                {
                    PropertyField("rolloffCurve");
                }
            }

            _serializedObject.ApplyModifiedProperties();
        }

        private void PropertyField(string propertyName, string label = null)
        {
            if (label != null)
            {
                EditorGUILayout.PropertyField(_serializedObject.FindProperty(propertyName), new GUIContent(label), true);
                return;
            }

            EditorGUILayout.PropertyField(_serializedObject.FindProperty(propertyName), true);
        }
    }
}