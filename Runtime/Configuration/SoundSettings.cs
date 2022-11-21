namespace Produktivkeller.SimpleAudioSolution.Configuration
{
    public static class SoundSettings
    {
        /// <summary>
        /// If this flag is set to true, the Unity spatialize flag on audio sources will never be set, even if specified by an audio event.
        /// </summary>
        public static bool IsSpatializeDisabled
        { get; set; }
    }
}
