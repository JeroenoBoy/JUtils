using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class ReadOnly : PropertyAttribute
    {
    }
    
    
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ReadOnly))]
    public class MyClass : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
    
#endif
}
