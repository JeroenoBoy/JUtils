using UnityEngine;



namespace JUtils.Extensions
{
    public static class AudioListenerExtensions
    {
        
        public static void RandomPitch(this AudioSource self, float min, float max)
        {
            self.pitch = Random.Range(min, max);
        }
    }
}
