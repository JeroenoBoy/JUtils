using UnityEngine;



namespace JUtils
{
    /// <summary>
    /// A interface that helps with weighted randomness <see cref="WeightedExtensions"/>
    /// </summary>
    public interface IWeighted
    {
        public float weight { get; }
    }
    
    
    /// <summary>
    /// A struct that makes it easier to work with weighted randomness <seealso cref="WeightedExtensions"/>
    /// </summary>
    [System.Serializable]
    public struct Weighted<T> : IWeighted
    {
        [SerializeField] private float _weight;
        [SerializeField] private T _value;

        public float weight => _weight;
        public T value => _value;


        /// <summary>
        /// Create a new instance of Weighted`T
        /// </summary>
        public Weighted(T initialValue, float weight)
        {
            _weight = weight;
            _value = initialValue;
        }
   }
}