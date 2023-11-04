using UnityEditor;
using UnityEngine;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);

            if (property.objectReferenceValue != null) return;
            EditorGUILayout.HelpBox($"Field \"{property.displayName}\" is required", MessageType.Error);
            EditorGUILayout.Space(4);
        }
    }
}