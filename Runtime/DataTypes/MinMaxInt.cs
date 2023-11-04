using UnityEngine;



namespace JUtils
{
    /// <summary>
    /// A struct with a custom editor which can be used to get a minimum and a maximum value, handy for random values & clamping 
    /// </summary>
    [System.Serializable]
    public struct MinMaxInt
    {
        [SerializeField] internal int _min;
        [SerializeField] internal int _max;

        /// <summary>
        /// Get the min value
        /// </summary>
        public int min => _min;
        
        /// <summary>
        /// Get the max value
        /// </summary>
        public int max => _max;


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
        public int Random(System.Random random)
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
    }
}