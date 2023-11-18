using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    public class OptionalEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty enabledProperty = property.FindPropertyRelative("_enabled");
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");
            
            VisualElement root = new();
            
            Toggle toggle = new();
            PropertyField propertyField = new(valueProperty, property.displayName);

            toggle.style.marginTop = 2;
            toggle.style.marginLeft = 4;
            toggle.style.maxHeight = 20;

            if (valueProperty.propertyType == SerializedPropertyType.Generic) {
                toggle.style.position = Position.Absolute;
                toggle.style.right = 0;
            }
            
            propertyField.style.flexGrow = 1;
            propertyField.SetEnabled(enabledProperty.boolValue);
            
            toggle.BindProperty(enabledProperty);
            toggle.RegisterValueChangedCallback(e => propertyField.SetEnabled(e.newValue));
            
            root.style.flexDirection = FlexDirection.Row;
            
            root.Add(propertyField);
            root.Add(toggle);

            return root;
        }
    }
}