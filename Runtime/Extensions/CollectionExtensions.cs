using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace JUtils
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Check if the collection is empty or null
        /// </summary>
        public static bool IsEmptyOrNull<T>(this ICollection<T> self)
        {
            return self == null || self.Count == 0;
        }


        /// <summary>
        /// Check if the collection is empty or null
        /// </summary>
        public static bool IsNotEmptyOrNull<T>(this ICollection<T> self)
        {
            return self != null && self.Count != 0;
        }


        /// <summary>
        /// Returns true if the collection is empty
        /// </summary>
        public static bool IsEmpty<T>(this ICollection<T> self)
        {
            return self.Count == 0;
        }


        /// <summary>
        /// Returns true if the collection is filled
        /// </summary>
        public static bool IsNotEmpty<T>(this ICollection<T> self)
        {
            return self.Count > 0;
        }


        /// <summary>
        /// Try get the element at that index, else return the <see cref="defaultValue"/>
        /// </summary>
        public static T GetOrElse<T>(this ICollection<T> self, int index, T defaultValue)
        {
            return self.Count > index ? self.ElementAt(index) : defaultValue;
        }
    }
}