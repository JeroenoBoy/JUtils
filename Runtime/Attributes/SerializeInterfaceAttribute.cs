using System;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Restrict the UnityEngine.Object field to extend a certain interface
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class SerializeInterfaceExample : MonoBehaviour
    ///     {
    ///         [SerializeInterface(typeof(IProcessor))]
    ///         [SerializeField, Required] private Object _processor;
    ///         public IProcessor processor => _processor as IProcessor;
    ///     }
    /// }
    /// </code></example>
    public class SerializeInterfaceAttribute : PropertyAttribute
    {
        public readonly Type type;
        public SerializeInterfaceAttribute(Type type)
        {
            this.type = type;
        }
    }
}
