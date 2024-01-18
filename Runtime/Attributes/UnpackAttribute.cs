using UnityEngine;

namespace JUtils
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
    public class UnpackAttribute : PropertyAttribute { }
}
