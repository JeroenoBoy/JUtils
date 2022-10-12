using System;
using System.Collections.Generic;
using System.Linq;



namespace JUtils.Extensions
{
    public static class Enumerable
    {
        public static T Random<T>(this IEnumerable<T> self)
        {
            var enumerable = self as T[] ?? self.ToArray();
            var size = enumerable.Length;
            return size == 0
                ? default
                : enumerable.ElementAt(UnityEngine.Random.Range(0, size));
        }
        
        
        public static T Random<T>(this IEnumerable<T> self, Random random)
        {
            var enumerable = self as T[] ?? self.ToArray();
            var size = enumerable.Length;
            return size == 0
                ? default
                : enumerable.ElementAt(random.Next(0, size));
        }
    }
}
