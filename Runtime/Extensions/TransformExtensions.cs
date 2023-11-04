using System.Collections.Generic;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Some useful extensions to transforms
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Enumerate thru all the children
        /// </summary>
        public static IEnumerable<Transform> GetChildren(this Transform self)
        {
            for (int i = 0; i < self.childCount; i++)
            {
                yield return self.GetChild(i);
            }
        }

        
        /// <summary>
        /// Get the closest transform from self
        /// </summary>
        /// <param name="a">The first target to check</param>
        /// <param name="b">The second target to check</param>
        /// <returns>The closest transform</returns>
        public static Transform Closest(this Transform self, Transform a, Transform b)
        {
            Vector3 pos = self.position;
            
            float d1 = (a.position - pos).sqrMagnitude;
            float d2 = (b.position - pos).sqrMagnitude;
            
            return d1 < d2 ? a : b;
        }
    }
}
