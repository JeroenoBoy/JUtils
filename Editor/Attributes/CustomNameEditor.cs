using UnityEditor;
using UnityEngine;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(CustomNameAttribute))]
    public class CustomNameEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.text = ((CustomNameAttribute)attribute).name;
            EditorGUI.PropertyField(position, property, label);
        }
    }
}