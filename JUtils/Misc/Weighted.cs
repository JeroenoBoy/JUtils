using System.Runtime.CompilerServices;
using UnityEditor;
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



#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Weighted<>))]
    internal class WeightedEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value"), label, true);
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty weightField = property.FindPropertyRelative("_weight");
            SerializedProperty valueField  = property.FindPropertyRelative("_value");
            
            if (valueField.propertyType == SerializedPropertyType.Generic)
                HandleManaged(position, property, label, weightField, valueField);
            else
                HandleUnmanaged(position, property, label, weightField, valueField);
            
            property.serializedObject.ApplyModifiedProperties();
        }


        private void HandleManaged(Rect position, SerializedProperty property, GUIContent label, SerializedProperty weightField, SerializedProperty valueField)
        {
            GUIContent labelCopy = new GUIContent(label);
            
            //  Drawing managed field

            Rect weightFieldRect = new Rect(position) {x = position.x + position.width - 48, width = 48, height = EditorGUI.GetPropertyHeight(weightField)};
            Rect valueFieldRect  = new Rect(position) {width = position.width - 50};
            
            EditorGUI.PropertyField(valueFieldRect, valueField, labelCopy, true);
            EditorGUI.PropertyField(weightFieldRect, weightField, GUIContent.none);
        }


        private void HandleUnmanaged(Rect position, SerializedProperty property, GUIContent label, SerializedProperty weightField, SerializedProperty valueField)
        {
            //  Drawing label
                        
            float labelWidth = EditorGUIUtility.labelWidth;
            
            Rect labelRect = new (position) {width = labelWidth};
            EditorGUI.LabelField(labelRect, label);
            
            //  Drawing weighted float
            
            float valueWidth = 48;
            
            Rect weightedFieldRect = new (position) { x = position.x + labelWidth, width =  valueWidth, height = EditorGUI.GetPropertyHeight(weightField)};
            weightField.floatValue = EditorGUI.FloatField(weightedFieldRect, weightField.floatValue);
            
            //  Drawing value

            Rect valueFieldRect = new (position) {x = position.x + labelWidth + valueWidth + 2, width = position.width - labelWidth - valueWidth - 2};
            EditorGUI.PropertyField(valueFieldRect, valueField, GUIContent.none);
        }
    }
#endif
    
    
    
    /// <summary>
    /// Contains helper functions for working with Weighted Randomness
    /// </summary>
    public static class WeightedExtensions
    {
        /// <summary>
        /// Get a random weighted value using UnityEngine.Random
        /// </summary>
        public static T Random<T>(this Weighted<T>[] weightedArray)
        {
            float sum = weightedArray.WeightedSum();
            float index = UnityEngine.Random.Range(0, sum);
            return weightedArray.GetWeightedValue(index).value;
        }


        /// <summary>
        /// Get a random weighted value using System.Random
        /// </summary>
        public static T Random<T>(this Weighted<T>[] weightedArray, System.Random random)
        {
            float sum = weightedArray.WeightedSum();
            float index = (float)(random.NextDouble() * sum);
            return weightedArray.GetWeightedValue(index).value;
        }


        /// <summary>
        /// Get a random weighted value using UnityEngine.Random
        /// </summary>
        public static T Random<T>(this T[] weightedArray) where T : IWeighted
        {
            float sum = weightedArray.WeightedSum();
            float index = UnityEngine.Random.Range(0, sum);
            return weightedArray.GetWeightedValue(index);
        }
        
        
        /// <summary>
        /// Get a random weighted value using System.Random
        /// </summary>
        public static T Random<T>(this T[] weightedArray, System.Random random) where T : IWeighted
        {
            float sum = weightedArray.WeightedSum();
            float index = (float)(random.NextDouble() * sum);
            return weightedArray.GetWeightedValue(index);
        }


        /// <summary>
        /// Get a weighted value at a certain index
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetWeightedValue<T>(this T[] weightedArray, float index) where T : IWeighted
        { 
            T value = default;

            for (int i = 0; index > 0; i++) {
                value = weightedArray[i];
                index -= weightedArray[i].weight;
            }

            return value;
        }

        
        /// <summary>
        /// Get the sum of a weighted array
        /// </summary>
        public static float WeightedSum<T>(this T[] weightedArray) where T : IWeighted
        {
            float sum = 0;
            for (int i = weightedArray.Length; i --> 0;) {
                sum += weightedArray[i].weight;
            }

            return sum;
        }
    }
}