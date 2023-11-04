using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace JUtils
{
    /// <summary>
    /// A dictionary of which the values do not get lost on serialization
    /// </summary>
    /// <remarks>Please note that TKey and TValue must be serializable as well</remarks>
    [System.Serializable]
    public class SerializableDictionary<TKey,TValue> : Dictionary<TKey,TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private Pair[] _pairs;


        /// <summary>
        /// Do not manually call
        /// </summary>
        public void OnBeforeSerialize()
        {
            TKey[] keys = Keys.ToArray();
            TValue[] values = Values.ToArray();

            _pairs = new Pair[keys.Length];

            for (int i = _pairs.Length; i --> 0;) {
                _pairs[i] = new Pair(keys[i], values[i]);
            }
        }


        /// <summary>
        /// Do not manually call
        /// </summary>
        public void OnAfterDeserialize()
        {
            Clear();
            for (int i = _pairs.Length; i --> 0;) {
                if (_pairs[i].key == null) {
                    Debug.LogError($"Key is null! -- {typeof(TKey)} might not be serializable");
                } else if (_pairs[i].value == null) {
                    Debug.LogError($"Value is null! -- {typeof(TValue)} might not be serializable");
                }
                else {
                    Add(_pairs[i].key, _pairs[i].value);
                }
            }
        }

        
        
        /// <summary>
        /// The pair object used for the internal data
        /// </summary>
        [System.Serializable]
        public struct Pair
        {
            public TKey key;
            public TValue value;

            public Pair(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}