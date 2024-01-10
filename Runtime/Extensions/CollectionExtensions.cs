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
        public static bool IsEmptyOrNull<T, TType>(this T self) where T : ICollection<TType>
        {
            return self == null || self.Count == 0;
        }


        /// <summary>
        /// Check if the collection is empty or null
        /// </summary>
        public static bool IsNotEmptyOrNull<T, TType>(this T self) where T : ICollection<TType>
        {
            return self != null && self.Count != 0;
        }


        /// <summary>
        /// Returns true if the collection is empty
        /// </summary>
        public static bool IsEmpty<T, TType>(this T self) where T : ICollection<TType>
        {
            return self.Count == 0;
        }


        /// <summary>
        /// Returns true if the collection is filled
        /// </summary>
        public static bool IsNotEmpty<T, TType>(this T self) where T : ICollection<TType>
        {
            return self.Count > 0;
        }


        public static T OrEmpty<T, TType>([CanBeNull] this T self) where T : ICollection<TType>
        {
            return self ?? Activator.CreateInstance<T>();
        }


        /// <summary>
        /// Try get the element at that index, else return the <see cref="defaultValue"/>
        /// </summary>
        public static TType GetOrElse<T, TType>(this T self, int index, TType defaultValue) where T : ICollection<TType>
        {
            return self.Count > index ? self.ElementAt(index) : defaultValue;
        }
    }
}