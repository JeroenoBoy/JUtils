using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    
    /// <summary>
    /// Disable writing for this property in the editor
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class ReadOnlyExample : MonoBehaviour
    ///     {
    ///         [SerializeField, ReadOnlyProperty] private float _timer;
    ///
    ///
    ///         private void Update()
    ///         {
    ///             _timer += Time.deltaTime * Random.value
    ///         }
    ///     }  
    /// }
    /// </code></example>
    public class ReadOnlyProperty : PropertyAttribute
    {
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ReadOnlyProperty))]
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
    

}
