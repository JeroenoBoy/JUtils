using System;
using UnityEngine;
using Random = System.Random;

namespace JUtils
{
    /// <summary>
    /// A struct with a custom editor which can be used to get a minimum and a maximum value, handy for random values & clamping 
    /// </summary>
    [Serializable]
    public struct MinMax
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        /// <summary>
        /// Get the min value
        /// </summary>
        public float min => _min;

        /// <summary>
        /// Get the max value
        /// </summary>
        public float max => _max;

        /// <summary>
        /// Returns _max - _min
        /// </summary>
        public float range => _max - _min;

        /// <summary>
        /// Create a new MinMax instance
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public MinMax(float min, float max)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Get a random value from min to max using UnityEngine.Random
        /// </summary>
        /// <returns></returns>
        public float Random()
        {
            return UnityEngine.Random.Range(_min, _max);
        }

        /// <summary>
        /// Get a random value from min to max using System.Random
        /// </summary>
        public float Random(Random random)
        {
            return (float)(random.NextDouble() * (_max - _min) + _min);
        }

        /// <summary>
        /// Check if the value is contained between min and max
        /// </summary>
        public bool Contains(float value)
        {
            return value > min && value < max;
        }

        /// <summary>
        /// Clamps the value to be between A and B
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float Clamp(float value)
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