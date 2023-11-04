using UnityEngine;

namespace JUtils
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
    public class CustomNameAttribute : PropertyAttribute
    {
        public readonly string name;


        public CustomNameAttribute(string name)
        {
            this.name = name;
        }
    }
}
