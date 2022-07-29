using Produktivkeller.SimpleAudioSolution.Access;
using Produktivkeller.SimpleAudioSolution.Event;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Editor.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SoundEvent))]
    public class SoundEventEditor : UnityEditor.Editor
    {
        private readonly string[]           _tabs = { "General", "Advanced" };
        
        private          SerializedProperty _volume;
        private          SerializedProperty _pitch;
        private          SerializedProperty _delay;
        private          SerializedProperty _spatialize;
        private          SerializedProperty _spatialBlend;
        private          SerializedProperty _maxInstances;
        private          SerializedProperty _stealingMode;
        private          SerializedProperty _isLooping;
        private          SerializedProperty _priority;
        private          SerializedProperty _audioMixerGroup;
        private          SerializedProperty _audioClips;
        private          SerializedProperty _multiSoundEventPlaymode;
        private          SerializedProperty _ignoreFirstDelay;
        private          SerializedProperty _dopplerLevel;
        private          SerializedProperty _spread;
        private          SerializedProperty _distance;
        private          SerializedProperty _volumeRolloff;
        private          SerializedProperty _rolloffCurve;
        private          SerializedProperty _stereoPan;
        private          int                _tabSelected;


        public void OnEnable()
        {
            _volume                  = serializedObject.FindProperty("volume");
            _pitch                   = serializedObject.FindProperty("pitch");
            _delay                   = serializedObject.FindProperty("delay");
            _spatialize              = serializedObject.FindProperty("spatialize");
            _spatialBlend            = serializedObject.FindProperty("spatialBlend");
            _maxInstances            = serializedObject.FindProperty("maxInstances");
            _stealingMode            = serializedObject.FindProperty("stealingMode");
            _isLooping               = serializedObject.FindProperty("isLooping");
            _priority                = serializedObject.FindProperty("priority");
            _audioMixerGroup         = serializedObject.FindProperty("audioMixerGroup");
            _audioClips              = serializedObject.FindProperty("audioClips");
            _multiSoundEventPlaymode = serializedObject.FindProperty("multiSoundEventPlaymode");
            _ignoreFirstDelay        = serializedObject.FindProperty("ignoreFirstDelay");
            _dopplerLevel            = serializedObject.FindProperty("dopplerLevel");
            _spread                  = serializedObject.FindProperty("spread");
            _distance                = serializedObject.FindProperty("distance");
            _volumeRolloff           = serializedObject.FindProperty("volumeRolloff");
            _rolloffCurve            = serializedObject.FindProperty("rolloffCurve");
            _stereoPan               = serializedObject.FindProperty("stereoPan");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            _tabSelected = GUILayout.Toolbar(_tabSelected, _tabs);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Required according to https://docs.unity3d.com/ScriptReference/Editor.html.
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            switch (_tabSelected)
            {
                case 0:
                    ShowGeneralPage();
                    break;
                case 1:
                    ShowAdvancedPage();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            
            if (EditorGUI.EndChangeCheck() && Application.isPlaying)
            {
                SoundAccess.GetInstance().SoundEventHasChangedInPlayMode((SoundEvent)target);
            }
        }

        private void ShowGeneralPage()
        {
            PropertyField(_volume);
            PropertyField(_pitch);
            PropertyField(_isLooping);
            
            if (_isLooping.boolValue)
            {
                PropertyField(_ignoreFirstDelay);
            }
            
            PropertyField(_spatialBlend, "Spatial Blend (2D = 0, 3D = 1)");
            PropertyField(_maxInstances);
            PropertyField(_stealingMode);
            PropertyField(_audioClips);

            if (_audioClips.arraySize > 1)
            {
                PropertyField(_multiSoundEventPlaymode, "Playmode");
            }

            if (_spatialBlend.floatValue != 0)
            {
                PropertyField(_dopplerLevel);
                PropertyField(_spread);
                PropertyField(_distance);
                PropertyField(_volumeRolloff);

                if (_volumeRolloff.enumValueIndex == (int)VolumeRolloff.Custom)
                {
                    PropertyField(_rolloffCurve);
                }
            }
        }
        
        private void ShowAdvancedPage()
        {
            PropertyField(_delay);
            PropertyField(_priority, "Priority (0 = High Priority)");
            PropertyField(_audioMixerGroup);
            PropertyField(_spatialize);
            PropertyField(_stereoPan);
        }
        
        private void PropertyField(SerializedProperty serializedProperty, string label = null)
        {
            if (label != null)
            {
                EditorGUILayout.PropertyField(serializedProperty, new GUIContent(label), true);
                return;
            }

            EditorGUILayout.PropertyField(serializedProperty, true);
        }
    }
}