using System;
using Produktivkeller.SimpleAudioSolution.Event;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Editor.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxSlider))]
    public class RangePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty serializedProperty, GUIContent label)
        {
            if (attribute is not MinMaxSlider minMaxSlider)
            {
                return;
            }

            float min = 0;
            float max = 0;
            
            if (serializedProperty.propertyType == SerializedPropertyType.Vector2)
            {
                Vector2 v = serializedProperty.vector2Value;
                min = v.x;
                max = v.y;
            }
            

            float sliderWidth = EditorGUIUtility.currentViewWidth - 115 - EditorGUIUtility.labelWidth;
            float xOffset     = 2;
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginProperty(rect, GUIContent.none, serializedProperty);
            EditorGUI.LabelField(rect, label);
            min = (float) Math.Round(EditorGUI.FloatField(new Rect(rect.x +  xOffset +EditorGUIUtility.labelWidth, rect.y, 40, rect.height), min), 2);
            
            EditorGUI.MinMaxSlider(new Rect(rect.x + xOffset + 45 + EditorGUIUtility.labelWidth, rect.y, sliderWidth, rect.height), ref min, ref max, minMaxSlider.min, minMaxSlider.max);
            
            max = (float) Math.Round(EditorGUI.FloatField(new Rect(rect.x + xOffset + 45 + sliderWidth + 5 + EditorGUIUtility.labelWidth, rect.y, 40, rect.height), max), 2);
            
            if (EditorGUI.EndChangeCheck())
            {
                using (SerializedProperty x = serializedProperty.FindPropertyRelative("x"))
                {
                    x.floatValue = (float) Math.Round(min, 2);
                }
                
                using (SerializedProperty y = serializedProperty.FindPropertyRelative("y"))
                {
                    y.floatValue = (float) Math.Round(max, 2);
                }
            }
            
            EditorGUI.EndProperty();
            EditorGUILayout.EndHorizontal();
        }
    }
}