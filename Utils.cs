using System;
using System.Collections.Generic;
using UnityEngine;

namespace JUtils
{
    public static class Utils
    {
        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Quaternion Quaternion(float? x = null, float? y = null, float? z = null)
        {
            return UnityEngine.Quaternion.Euler(x ?? 0, y ?? 0, z ?? 0);
        }
    }
}