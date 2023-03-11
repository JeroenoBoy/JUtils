﻿using UnityEditor;
using UnityEngine;



namespace JUtils
{
    [System.Serializable]
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
        /// Get a ranbdom value from min to max using System.Random
        /// </summary>
        public float Random(System.Random random)
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
        

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(MinMax))]
        public class RangeEditor : PropertyDrawer
        {
            private GUIContent[] _labels = new[]
            {
                new GUIContent("Min"),
                new GUIContent("Max")
            };

            private float[] _values = new float[2];
            
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                SerializedProperty minField = property.FindPropertyRelative(nameof(_min));
                SerializedProperty maxField = property.FindPropertyRelative(nameof(_max));

                _values[0] = minField.floatValue;
                _values[1] = maxField.floatValue;
                
                EditorGUI.MultiFloatField(position, label, _labels, _values);

                minField.floatValue = _values[0];
                maxField.floatValue = _values[1];
                
                property.serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}
