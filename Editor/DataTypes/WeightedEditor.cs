using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(Weighted<>))]
    public class WeightedEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty weightField = property.FindPropertyRelative("_weight");
            SerializedProperty valueField = property.FindPropertyRelative("_value");

            VisualElement root = new();
            
            FloatField weight = new();
            PropertyField propertyField = new(valueField, property.displayName);

            weight.BindProperty(weightField);
            
            root.style.flexDirection = FlexDirection.Row;
            propertyField.style.flexGrow = 1;
            weight.style.width = 64;

            if (valueField.propertyType == SerializedPropertyType.Generic) {
                weight.style.position = Position.Absolute;
                weight.style.right = 0;
            }
            
            root.Add(propertyField);
            root.Add(weight);

            return root;
        }
    }
}