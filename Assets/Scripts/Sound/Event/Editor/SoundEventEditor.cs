using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;


namespace Sound.Event
{
    [CustomEditor(typeof(SoundEvent),true)]
    public class SoundEventEditor : Editor
    {

        private float randomVolume;
        private float randomPitch;

        private AnimBool isRandomVolume = new AnimBool(false);
        private AnimBool isRandomPitch = new AnimBool(false);

        private SoundEvent _soundEvent;


        public void OnEnable()
        {
            _soundEvent = (SoundEvent)target;

            isRandomPitch.valueChanged.AddListener(Repaint);
            isRandomVolume.valueChanged.AddListener(Repaint);
        }


        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            ToggleRandomVolume();
            ToggleRandomPitch();

            //RandomizerCheckbox(isRandomPitch,"Randomize Pitch", 1,-3,3,1,_soundEvent.randomizePitch);
            //RandomizerCheckbox(isRandomVolume, "Randomize Volume", 1,0,1,1,_soundEvent.randomizePitch);
        }

        public void ToggleRandomPitch()
        {

            isRandomPitch.target = EditorGUILayout.ToggleLeft("Randomize Pitch", isRandomPitch.target);


            if (EditorGUILayout.BeginFadeGroup(isRandomPitch.faded))
            {
                EditorGUI.indentLevel++;

                randomPitch = EditorGUILayout.Slider(randomPitch, -3f, 3f);

                EditorGUI.indentLevel--;
            }
            else
            {
                randomPitch = 1f;
            }

            _soundEvent.randomizePitch = randomPitch;
            EditorGUILayout.EndFadeGroup();

        }

        public void ToggleRandomVolume()
        {

            isRandomVolume.target = EditorGUILayout.ToggleLeft("Randomize Volume", isRandomVolume.target);


            if (EditorGUILayout.BeginFadeGroup(isRandomVolume.faded))
            {
                EditorGUI.indentLevel++;

                randomVolume = EditorGUILayout.Slider(randomVolume, 0f, 1f);

                EditorGUI.indentLevel--;
            }
            else
            {
                randomVolume = 1f;
            }

            _soundEvent.randomizeVolume = randomVolume;
            EditorGUILayout.EndFadeGroup();
        }
        
        public void RandomizerCheckbox(AnimBool isRandom, string name, float set, float min, float max, float reset, float goal)
        {
            isRandom.valueChanged.AddListener(Repaint);
            isRandom.target = EditorGUILayout.ToggleLeft(name, isRandom.target);


            if (EditorGUILayout.BeginFadeGroup(isRandom.faded))
            {
                EditorGUI.indentLevel++;

                randomVolume = EditorGUILayout.Slider(set, min, max);

                EditorGUI.indentLevel--;
            }
            else
            {
                randomVolume = reset;
            }

            goal = set;
            EditorGUILayout.EndFadeGroup();
        }
        
    }

}

