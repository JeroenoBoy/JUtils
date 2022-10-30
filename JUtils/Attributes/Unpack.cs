using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class Unpack : PropertyAttribute
    {
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(Unpack))]
        private class UnpackEditor : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                property.NextVisible(true);
                float height = 0;

                do {
                    height += EditorGUI.GetPropertyHeight(property) + 2;
                } while (property.NextVisible(false));

                return height;
            }


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                property.NextVisible(true);

                do {
                    float height = position.height = EditorGUI.GetPropertyHeight(property);
                    EditorGUI.PropertyField(position, property, true);
                    position.y += height + 2;

                } while (property.NextVisible(false));
            }
        }
#endif
    }
}
