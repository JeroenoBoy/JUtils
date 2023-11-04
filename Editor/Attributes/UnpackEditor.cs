using UnityEditor;
using UnityEngine;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(UnpackAttribute))]
    public class UnpackEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property.NextVisible(true);
            float height = 0;
            int depth = property.depth;

            do {
                height += EditorGUI.GetPropertyHeight(property) + 2;
            } while (property.NextVisible(false) && property.depth == depth);

            return height;
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.NextVisible(true);
            int depth = property.depth;

            do {
                float height = position.height = EditorGUI.GetPropertyHeight(property);
                EditorGUI.PropertyField(position, property, true);
                position.y += height + 2;

            } while (property.NextVisible(false) && property.depth == depth);
        }
    }
}