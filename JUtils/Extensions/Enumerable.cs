using System;
using System.Collections.Generic;
using System.Linq;



namespace JUtils.Extensions
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
            T[] enumerable = self as T[] ?? self.ToArray();
            int size       = enumerable.Length;
            
            return size == 0
                ? default
                : enumerable.ElementAt(UnityEngine.Random.Range(0, size));
        }
        
        
        /// <summary>
        /// Get a random element from the enumerable using System.Random
        /// </summary>
        public static T Random<T>(this IEnumerable<T> self, Random random)
        {
            T[] enumerable = self as T[] ?? self.ToArray();
            int size       = enumerable.Length;
            
            return size == 0
                ? default
                : enumerable.ElementAt(random.Next(0, size));
        }


        /// <summary>
        /// Get the index of an element which matches the comparer
        /// </summary>
        public static int IndexOf<T>(this IEnumerable<T> self, Func<T, bool> comparer)
        {
            int i = 0;
            foreach (T x in self) {
                if (comparer(x)) return i;
                i++;
            }

            return -1;
        }
    }
}
