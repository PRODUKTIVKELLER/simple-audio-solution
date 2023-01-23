namespace Produktivkeller.SimpleAudioSolution.Persistence
{
    public interface ISimpleAudioSolutionPersistence
    {
        public bool HasKey(string key);

        public void SetFloat(string key, float value);

        public float GetFloat(string key);

        public void Save();
    }
}