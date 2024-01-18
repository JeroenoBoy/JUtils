using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Disable writing for this property in the editor
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class ReadOnlyExample : MonoBehaviour
    ///     {
    ///         [SerializeField, Uneditable] private float _timer;
    ///
    ///
    ///         private void Update()
    ///         {
    ///             _timer += Time.deltaTime * Random.value
    ///         }
    ///     }
    /// }
    /// </code></example>
    public class UneditableAttribute : PropertyAttribute { } 
}
