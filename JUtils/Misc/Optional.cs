using UnityEditor;
using UnityEngine;


namespace JUtils
{
    /// <summary>
    /// A struct useful for showing in the inspector if the value is optional, and does not have to be set. Also allows for quicker checks if the value is set
    /// </summary>
    /// <example><code>
    /// namespace Example
    /// {
    ///     public class OptionalExample : MonoBehaviour
    ///     {
    ///         [SerializeField] private Optional&#60;HealthComponent> _healthComponent;
    ///
    ///         private void Start()
    ///         {
    ///             HealthComponent hc = _healthComponent ? _healthComponent : GetComponent&#60;HealthComponent>();
    ///             hc.Damage(10);
    ///         }
    ///     }
    /// }
    /// </code></example>
    [System.Serializable]
    public struct Optional<T>
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private T _value;

        public bool enabled => _enabled && _value is not null;
        public T    value   => _value;


        public Optional(T initialValue)
        {
            _enabled = initialValue is not null;
            _value   = initialValue;
        }


        public static implicit operator bool(Optional<T> optional) => optional.enabled;
        public static implicit operator T   (Optional<T> optional) => optional._value;
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
                    x = position.x + 2 + valueRect.width - (property.depth >= 1 ? 14 : 0),
                    y = position.y,
                    width = enabledHeight,
                    height = enabledHeight
                };
                
                //  Drawing gui things
                
                EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
                EditorGUI.PropertyField(valueRect, valueProperty, ogLabel, true);
                EditorGUI.EndDisabledGroup();
                
                EditorGUI.PropertyField(toggleRect, enabledProperty, GUIContent.none);
            }
        }
#endif
}
