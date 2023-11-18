using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// When an UnityEngine.Object reference has not been set, show a big warning
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class RequiredExample : MonoBehaviour
    ///     {
    ///         [SerializeField, Required] private GameObject _bulletPrefab;
    ///     }
    /// }
    /// </code></example>
    public class RequiredAttribute : PropertyAttribute { }
}
