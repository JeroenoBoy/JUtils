using System;
using UnityEngine;
using Random = System.Random;

namespace JUtils
{
    /// <summary>
    /// A struct with a custom editor which can be used to get a minimum and a maximum value, handy for random values & clamping 
    /// </summary>
    [Serializable]
    public struct MinMaxInt
    {
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        /// <summary>
        /// Get the min value
        /// </summary>
        public int min => _min;

        /// <summary>
        /// Get the max value
        /// </summary>
        public int max => _max;

        /// <summary>
        /// Returns _max - _min
        /// </summary>
        public int range => _max - _min;

        /// <summary>
        /// Create a new MinMax instance
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public MinMaxInt(int min, int max)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Get a random value from min to max using UnityEngine.Random
        /// </summary>
        /// <returns></returns>
        public int Random()
        {
            return UnityEngine.Random.Range(_min, _max);
        }

        /// <summary>
        /// Get a ranbdom value from min to max using System.Random
        /// </summary>
        public int Random(Random random)
        {
            return (int)(random.NextDouble() * (_max - _min) + _min);
        }

        /// <summary>
        /// Check if the value is contained between min and max
        /// </summary>
        public bool Contains(int value)
        {
            return value > min && value < max;
        }

        /// <summary>
        /// Clamps the value to be between A and B
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Clamp(int value)
        {
            return Mathf.Clamp(value, min, max);
        }

        /// <summary>
        /// Lerp from min to max
        /// </summary>
        public float Lerp(float time)
        {
            return Mathf.Lerp(min, max, time);
        }

        /// <summary>
        /// inverse lerp from min to max
        /// </summary>
        public float InverseLerp(float value)
        {
            return Mathf.InverseLerp(min, max, value);
        }
    }
}