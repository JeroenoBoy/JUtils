using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    /// <summary>
    /// Displays the given type in the inspector as if it isn't a different object. Useful for Unity DOTS baker workflow
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class UnpackExample : MonoBehaviour
    ///     {
    ///         [SerializeField, Unpack] private EnemyData _enemyData;
    ///     }  
    /// }
    /// </code></example>
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
#endif
    }
}
