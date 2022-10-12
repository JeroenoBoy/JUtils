using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.Attributes
{
    [System.Serializable]
    public struct Optional<T>
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private T _value;

        public bool enabled => _enabled;
        public T value => _value;


        public Optional(T initialValue)
        {
            _enabled = true;
            _value = initialValue;
        }
    }
    
    
#if UNITY_EDITOR


        [CustomPropertyDrawer(typeof(Optional<>))]
        public class OptionalEditor : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                SerializedProperty valueProperty = property.FindPropertyRelative("_value");
                return EditorGUI.GetPropertyHeight(valueProperty);
            }


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {

                SerializedProperty enabledProperty = property.FindPropertyRelative("_enabled");
                SerializedProperty valueProperty   = property.FindPropertyRelative("_value");

                position.width -= 24;
                EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
                EditorGUI.PropertyField(position, valueProperty, label, true);
                EditorGUI.EndDisabledGroup();
                
                position.x += position.width + 24;
                position.width = position.height = EditorGUI.GetPropertyHeight(enabledProperty);
                position.x -= position.width;
                EditorGUI.PropertyField(position, enabledProperty, GUIContent.none);
            }
        }
#endif
}
