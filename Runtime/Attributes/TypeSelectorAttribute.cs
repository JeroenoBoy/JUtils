using UnityEngine;


namespace JUtils
{
    /// <summary>
    /// Show a dropdown for all classes that extend the field's type. Requires SerializeReference attribute to function correctly
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class TypeSelectorTest : MonoBehaviour
    ///     {
    ///         [SerializeReference, TypeSelector] public IAnimal _animal;
    ///     }
    ///      
    ///     public interface IAnimal {
    ///         string name { get; }
    ///     }
    ///      
    ///     public class Dog : IAnimal
    ///     {
    ///         [field: SerializeField]
    ///         public string name { get; set; }
    ///         
    ///         [field: SerializeField]
    ///         public string bark { get; set; }
    ///     }
    ///      
    ///     public class Cat : IAnimal
    ///     {
    ///         [field: SerializeField]
    ///         public string name { get; set; }
    ///         
    ///         [field: SerializeField]
    ///         public string meow { get; set; }
    ///     }
    /// }
    /// </code></example>
    public class TypeSelectorAttribute : PropertyAttribute { }
}