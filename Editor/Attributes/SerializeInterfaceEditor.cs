using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(SerializeInterfaceAttribute))]
    public class SerializeInterfaceEditor : PropertyDrawer
    {
        private new SerializeInterfaceAttribute attribute => base.attribute as SerializeInterfaceAttribute;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.type != "PPtr<$Object>") throw new Exception("Field must be of type Object");

            ObjectField field = new(property.displayName) { objectType = attribute.type, allowSceneObjects = true, value = property.objectReferenceValue };
            field.BindProperty(property);

            return field;
        }
    }
}