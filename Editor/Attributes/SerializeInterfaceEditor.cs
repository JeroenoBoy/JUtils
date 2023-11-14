using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
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

            ObjectField field = new(property.name) {
                objectType = attribute.type,
                allowSceneObjects = true,
                value = property.objectReferenceValue
            };

            field.RegisterValueChangedCallback(e => property.objectReferenceValue = e.newValue);
            return field;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializeInterfaceAttribute target = attribute;
        
            property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, target.type, true);
        }
    }
}