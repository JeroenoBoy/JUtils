﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace JUtils
{
    [System.Serializable]
    public class SerializableDictionary<TKey,TValue> : Dictionary<TKey,TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private Pair[] _pairs;


        public void OnBeforeSerialize()
        {
            TKey[] keys = Keys.ToArray();
            TValue[] values = Values.ToArray();

            _pairs = new Pair[keys.Length];

            for (int i = _pairs.Length; i-- > 0;) {
                _pairs[i] = new Pair(keys[i], values[i]);
            }
        }


        public void OnAfterDeserialize()
        {
            Clear();
            for (int i = _pairs.Length; i-- > 0;) {
                Add(_pairs[i].key, _pairs[i].value);
            }

            _pairs = Array.Empty<Pair>();
        }


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
