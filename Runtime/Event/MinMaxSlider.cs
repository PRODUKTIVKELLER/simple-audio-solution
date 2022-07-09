using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Event
{
    public class MinMaxSlider : PropertyAttribute
    {
        public float min;
        public float max;

        public MinMaxSlider(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}