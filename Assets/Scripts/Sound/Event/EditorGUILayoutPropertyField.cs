using UnityEngine;
using UnityEditor;
using Sound.Event;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(SoundEvent))]
[CanEditMultipleObjects]
public class EditorGUILayoutPropertyField : Editor
{
    SerializedProperty randomizePitch;
    SerializedProperty randomizeVolume;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        randomizePitch = serializedObject.FindProperty("randomizePitch");
        randomizeVolume = serializedObject.FindProperty("randomizeVolume");
        Debug.Log("HEY");
    }

    public override void OnInspectorGUI()
    {
        //The variables and GameObject from the SoundEvent script are displayed in the Inspector with appropriate labels
        EditorGUILayout.PropertyField(randomizePitch, new GUIContent("Randomize"), GUILayout.Height(20));
        EditorGUILayout.PropertyField(randomizeVolume, new GUIContent("Randomize"));
        Debug.Log("HO");

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}