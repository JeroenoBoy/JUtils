using System.Collections.Generic;

using UnityEngine;



namespace JUtils.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Enumerate thru all the children
        /// </summary>
        public static IEnumerable<Transform> GetChildren(this Transform self)
        {
            for (var i = 0; i < self.childCount; i++)
            {
                yield return self.GetChild(i);
            }
        }

        /// <summary>
        /// Get the closest transform from self
        /// </summary>
        /// <param name="a">The first target to check</param>">
        /// <param name="b">The second target to check</param>
        /// <returns>The closest transform</returns>
        public static Transform Closest(this Transform self, Transform a, Transform b)
        {
            var pos = self.position;
            var d1 = (a.position - pos).sqrMagnitude;
            var d2 = (b.position - pos).sqrMagnitude;
            return d1 < d2 ? a : b;
        }
    }
}
