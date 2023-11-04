using System;
using UnityEditor;
using UnityEngine;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(SerializeInterfaceAttribute))]
    public class SerializeInterfaceEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializeInterfaceAttribute target = attribute as SerializeInterfaceAttribute;
            if (property.type != "PPtr<$Object>") throw new Exception("Field must be of type Object");
        
            property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, target.type, true);
        }
    }
}