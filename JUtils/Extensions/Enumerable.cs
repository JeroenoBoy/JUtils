using System;
using System.Collections.Generic;
using System.Linq;



namespace JUtils.Extensions
{
    public static class Enumerable
    {
        public static T Random<T>(this IEnumerable<T> self)
        {
            T[] enumerable = self as T[] ?? self.ToArray();
            int size       = enumerable.Length;
            
            return size == 0
                ? default
                : enumerable.ElementAt(UnityEngine.Random.Range(0, size));
        }
        
        
        public static T Random<T>(this IEnumerable<T> self, Random random)
        {
            T[] enumerable = self as T[] ?? self.ToArray();
            int size       = enumerable.Length;
            
            return size == 0
                ? default
                : enumerable.ElementAt(random.Next(0, size));
        }


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
