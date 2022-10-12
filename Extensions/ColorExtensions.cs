using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace JUtils.Extensions
{
    public static class ColorExtensions
    {
        private static bool CompareThreshold(float a, float b, float threshold)
        {
            return Mathf.Abs(a - b) < threshold;
        }


        /// <summary>
        /// Compare 2 colors with a threshold
        /// </summary>
        public static bool Equals(this Color self, Color other, float threshold)
        {
            return CompareThreshold(self.r, other.r, threshold) &&
                   CompareThreshold(self.g, other.g, threshold) &&
                   CompareThreshold(self.b, other.b, threshold);
        }


        /// <summary>
        /// Compare 2 colors with a threshold, probably worse but idk
        /// </summary>
        public static bool Equals2(this Color self, Color other, float threshold)
        {
            return CompareThreshold(
                self.r + self.g + self.b,
                other.r + other.g + other.b,
                threshold
            );
        }


        /// <summary>
        /// Compare 2 colors with a threshold, probably worse but idk
        /// </summary>
        public static bool TContains(
            this IEnumerable<Color> self,
            Color other,
            float threshold)
        {
            return self.Any(color => color.Equals(other, threshold));
        }
    }
}
