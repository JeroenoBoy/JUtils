using System;
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
        public static TType Random<T, TType>(this T self) where T : IEnumerable<TType>
        {
            IList<TType> list = self as IList<TType> ?? self.ToArray();
            int size = list.Count;

            return size == 0
                ? default
                : list[UnityEngine.Random.Range(0, size)];
        }


        /// <summary>
        /// Get a random element from the enumerable using System.Random
        /// </summary>
        public static TType Random<T, TType>(this T self, Random random) where T : IEnumerable<TType>
        {
            IList<TType> list = self as IList<TType> ?? self.ToArray();
            int size = list.Count;

            return size == 0
                ? default
                : list[random.Next(0, size)];
        }


        /// <summary>
        /// Returns a <see cref="HashSet{T}"/> that only contains the unique elements within an enumerator
        /// </summary>
        public static HashSet<TType> Distinct<T, TType>(this T self, int preFill = 16) where T : IEnumerable<TType>
        {
            HashSet<TType> set = new(preFill);
            foreach (TType type in self) {
                set.Add(type);
            }

            return set;
        }
    }
}