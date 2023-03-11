using UnityEditor;
using UnityEngine;



namespace JUtils
{
    [System.Serializable]
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
        

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(MinMaxInt))]
        public class RangeEditor : PropertyDrawer
        {
            private GUIContent[] _labels = new[]
            {
                new GUIContent("Min"),
                new GUIContent("Max")
            };

            private int[] _values = new int[2];
            
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                SerializedProperty minField = property.FindPropertyRelative(nameof(_min));
                SerializedProperty maxField = property.FindPropertyRelative(nameof(_max));

                bool mode = EditorGUIUtility.wideMode;
                EditorGUIUtility.wideMode = false;
                
                EditorGUI.LabelField(position, label);
                
                _values[0] = minField.intValue;
                _values[1] = maxField.intValue;

                Rect rect = new Rect(position) { x = position.x + EditorGUIUtility.labelWidth + 2, width = position.width - EditorGUIUtility.labelWidth - 2};
                EditorGUI.MultiIntField(rect, _labels, _values);

                minField.intValue = _values[0];
                maxField.intValue = _values[1];

                EditorGUIUtility.wideMode = mode;
                
                property.serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}