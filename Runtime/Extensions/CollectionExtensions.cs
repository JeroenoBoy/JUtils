using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

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


        /// <summary>
        /// Shuffles an list or array without creating a new one
        /// </summary>
        public static void Shuffle<T>(this IList<T> self)
        {
            int length = self.Count;
            for (int i = length; i > -1; i--) {
                int target = self.Count;
                (self[target], self[i]) = (self[i], self[target]);
            }
        }


        /// <summary>
        /// Pop a random item from the list
        /// </summary>
        [CanBeNull]
        public static T PopRandom<T>(this List<T> self)
        {
            return TryPopRandom(self, out T result) ? result : default;
        }


        /// <summary>
        /// Pop a random item from the list
        /// </summary>
        public static bool TryPopRandom<T>(this List<T> self, out T result)
        {
            if (self.Count == 0) {
                result = default;
                return false;
            }

            int i = Random.Range(0, self.Count);
            result = self[i];
            self.RemoveAt(i);
            return true;
        }
    }
}