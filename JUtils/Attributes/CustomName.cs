using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    /// <summary>
    /// Change the name of a parameter in the inspector
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class CustomNameExample : MonoBehaviour
    ///     {
    ///         [Header("Spawn Settings")]
    ///         [SerializeField, CustomName("Interval")] private float _spawnInterval; // will display as "Interval" in the inspector
    ///     }
    /// }
    /// </code></example>
    public class CustomName : PropertyAttribute
    {
        private readonly string _name;


        public CustomName(string name)
        {
            _name = name;
        }
        
        
        #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(CustomName))]
        private class NameEditor : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                label.text = ((CustomName)attribute)._name;
                EditorGUI.PropertyField(position, property, label);
            }
        }
        #endif
    }
}
