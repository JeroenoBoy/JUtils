using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JUtils
{
    /// <summary>
    /// Extensions for Enumerates
    /// </summary>
    public static class Enumerable
    {
        /// <summary>
        /// Get a random element from the enumerable
        /// </summary>
        public static T Random<T>(this IEnumerable<T> self)
        {
            IList<T> list = self as IList<T> ?? self.ToArray();
            int size = list.Count;

            return size == 0
                ? default
                : list[UnityEngine.Random.Range(0, size)];
        }


        /// <summary>
        /// Get a random element from the enumerable using System.Random
        /// </summary>
        public static T Random<T>(this IEnumerable<T> self, Random random)
        {
            IList<T> list = self as IList<T> ?? self.ToArray();
            int size = list.Count;

            return size == 0
                ? default
                : list[random.Next(0, size)];
        }


        /// <summary>
        /// Get the index of an element
        /// </summary>
        public static int IndexOf<T>(this IEnumerable<T> self, Predicate<T> predicate)
        {
            int i = 0;
            foreach (T element in self) {
                if (predicate(element)) return i;
                i++;
            }

            return -1;
        }
    }
}