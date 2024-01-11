using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Gives threshold comparing functions for Colors
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Compare 2 colors with a threshold, checks the difference between all separate RGB values
        /// </summary>
        public static bool Equals(this Color self, Color other, float threshold)
        {
            return CompareThreshold(self.r, other.r, threshold) &&
                   CompareThreshold(self.g, other.g, threshold) &&
                   CompareThreshold(self.b, other.b, threshold);
        }


        /// <summary>
        /// Compare 2 colors with a threshold, combines the colors and compares
        /// </summary>
        public static bool EqualsCompareTogether(this Color self, Color other, float threshold)
        {
            return CompareThreshold(
                self.r + self.g + self.b,
                other.r + other.g + other.b,
                threshold
            );
        }


        /// <summary>
        /// Check if all colors in an enumerable matches the other color. Using <see cref="Equals"/>
        /// </summary>
        public static bool Contains(this IEnumerable<Color> self, Color other, float threshold)
        {
            return self.Any(color => color.Equals(other, threshold));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CompareThreshold(float a, float b, float threshold)
        {
            return Mathf.Abs(a - b) < threshold;
        }
    }
}