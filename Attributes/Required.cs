using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class Required : PropertyAttribute
    {
#if UNITY_EDITOR

        [CustomPropertyDrawer(typeof(Required))]
        private class RequiredEditor : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.PropertyField(position, property, label);

                if (property.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox($"Field \"{property.displayName}\" is required", MessageType.Error);
                    EditorGUILayout.Space(4);
                }
            }
        }
        
#endif
    }
}
