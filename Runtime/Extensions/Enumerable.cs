using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

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


        /// <summary>
        /// Get the index of an element
        /// </summary>
        public static int IndexOf<T>(this IEnumerable<T> self, T element) where T : class
        {
            int i = 0;
            foreach (T elementToTest in self) {
                if (element == elementToTest) return i;
                i++;
            }

            return -1;
        }


        /// <summary>
        /// Get the nearest object from a certain position
        /// </summary>
        [CanBeNull]
        public static T Nearest<T>(this IEnumerable<T> self, Vector3 position) where T : Component
        {
            T nearestComponent = null;
            float nearestSqrDistance = float.MaxValue;
            foreach (T component in self) {
                float sqrDistance = (component.transform.position - position).sqrMagnitude;
                if (sqrDistance > nearestSqrDistance) continue;
                nearestComponent = component;
                nearestSqrDistance = sqrDistance;
            }

            return nearestComponent;
        }


        /// <summary>
        /// Get the furthest object from a certain position
        /// </summary>
        [CanBeNull]
        public static T Furthest<T>(this IEnumerable<T> self, Vector3 position) where T : Component
        {
            T nearestComponent = null;
            float nearestSqrDistance = 0f;
            foreach (T component in self) {
                float sqrDistance = (component.transform.position - position).sqrMagnitude;
                if (sqrDistance < nearestSqrDistance) continue;
                nearestComponent = component;
                nearestSqrDistance = sqrDistance;
            }

            return nearestComponent;
        }


        /// <summary>
        /// Try get an specific element in the list based on a predicate
        /// </summary>
        public static bool TryGet<T>(this IEnumerable<T> self, out T result, Predicate<T> predicate)
        {
            foreach (T item in self) {
                if (!predicate(item)) continue;
                result = item;
                return true;
            }

            result = default;
            return false;
        }
    }
}