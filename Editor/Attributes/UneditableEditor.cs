using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(UneditableAttribute))]
    public class UneditableEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField propertyField = new(property);
            propertyField.SetEnabled(false);
            return propertyField;
        }
    }
}