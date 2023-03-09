using UnityEditor;
using UnityEngine;



namespace JUtils
{
    [System.Serializable]
    public struct MinMax
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float min => _min;
        public float max => _max;


        public MinMax(float min, float max)
        {
            _min = min;
            _max = max;
        }


        public float Random()
        {
            return UnityEngine.Random.Range(_min, _max);
        }


        public float Random(System.Random random)
        {
            return (float)(random.NextDouble() * (_max - _min) + _min);
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
