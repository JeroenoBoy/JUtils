using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Adds extensions for the AudioSource component
    /// </summary>
    public static class AudioListenerExtensions
    {
        /// <summary>
        /// Add a random pitch from min to max on the component
        /// </summary>
        public static void RandomPitch(this AudioSource self, float min, float max)
        {
            self.pitch = Random.Range(min, max);
        }
    }
}
