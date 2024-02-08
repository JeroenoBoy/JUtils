using JetBrains.Annotations;
using UnityEngine;

namespace JUtils
{
    public static class UnityObjectExtensions
    {
        /// <summary>
        /// Will return null if the object is null, or if it has been destroyed. Also prevents warning when using ?. or ??
        /// </summary>
        [CanBeNull]
        public static T OrNull<T>([CanBeNull] this T obj) where T : Object
        {
            return obj == null ? null : obj;
        }
    }
}