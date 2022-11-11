using JUtils.Editor;
using UnityEditor;
using UnityEngine;


namespace JUtils.Attributes
{
    [System.Serializable]
    public struct Optional<T>
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private T _value;

        public bool enabled => _enabled;
        public T    value   => _value;


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
                GUIContent ogLabel = new (label.text, label.image, label.tooltip);

                //  Getting properties
                
                SerializedProperty enabledProperty = property.FindPropertyRelative("_enabled");
                SerializedProperty valueProperty   = property.FindPropertyRelative("_value");

                float enabledHeight = EditorGUI.GetPropertyHeight(enabledProperty);

                //  Creating rects

                Rect valueRect = new ()
                {
                    x = position.x,
                    y = position.y,
                    width = position.width - 20,
                    height = position.height
                };

                Rect toggleRect = new ()
                {
                    x = position.x + valueRect.width + 2 ,// + 2 + valueRect.width - (property.depth >= 1 ? 14 : 0),
                    y = position.y,
                    width = enabledHeight,
                    height = enabledHeight
                };
                
                //  Drawing gui things
                
                EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
                JUtilsEditor.PropertyField(valueRect, valueProperty, ogLabel);
                EditorGUI.EndDisabledGroup();
                
                EditorGUI.PropertyField(toggleRect, enabledProperty, GUIContent.none);
            }
        }
#endif
}
