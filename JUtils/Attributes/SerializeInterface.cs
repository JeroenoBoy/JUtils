using System;
using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class SerializeInterface : PropertyAttribute
    {
        private readonly Type _type;
        public SerializeInterface(Type type)
        {
            _type = type;
        }


        
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(SerializeInterface))]
        public class SerializeInterfaceEditor : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                SerializeInterface target = attribute as SerializeInterface;
                if (property.type != "PPtr<$Object>") throw new Exception("Field must be of type Object");
            
                property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, target._type, true);
            }
        }
#endif
    }
}
