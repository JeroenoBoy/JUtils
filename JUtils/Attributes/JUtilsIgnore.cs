using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class JUtilsIgnore : PropertyAttribute
    {
        [CustomPropertyDrawer(typeof(JUtilsIgnore))]
        private class Editor : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}