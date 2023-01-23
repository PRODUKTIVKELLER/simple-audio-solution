using UnityEngine;

namespace Produktivkeller.SimpleAudioSolution.Persistence
{
    public class SimpleAudioSolutionPersistence : ISimpleAudioSolutionPersistence
    {
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}