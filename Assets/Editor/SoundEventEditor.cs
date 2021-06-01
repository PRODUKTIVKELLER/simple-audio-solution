using UnityEditor;

namespace Sound.Event.Editor
{
    [CustomEditor(typeof(SoundEvent), true)]
    public class SoundEventEditor : UnityEditor.Editor
    {
        private float _randomVolume;

        private bool _randomVolumeActive;

        private SoundEvent _soundEvent;


        public void OnEnable()
        {
            _soundEvent = (SoundEvent) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ToggleRandomVolume();
            ToggleRandomPitch();
        }

        private void ToggleRandomPitch()
        {
            _soundEvent.randomizePitchActive = EditorGUILayout.ToggleLeft("Randomize Pitch", _soundEvent.randomizePitchActive);

            _soundEvent.randomizePitch = _soundEvent.randomizePitchActive ? EditorGUILayout.Slider(_soundEvent.randomizePitch, -3f, 3f) : 1f;
        }

        private void ToggleRandomVolume()
        {
            _randomVolumeActive = EditorGUILayout.ToggleLeft("Randomize Volume", _randomVolumeActive);

            _randomVolume = _randomVolumeActive ? EditorGUILayout.Slider(_randomVolume, 0f, 1f) : 1f;

            _soundEvent.randomizeVolume = _randomVolume;
        }
    }
}