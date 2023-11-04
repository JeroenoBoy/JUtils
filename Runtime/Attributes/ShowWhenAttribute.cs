using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Hide a field if the condition does not match, allows checks for bools, ints, floats & strings
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class ShowWhenExample : MonoBehaviour
    ///     {
    ///         [SerializeField] private bool _autoSet;
    /// 
    ///         [ShowWhen(nameof(_autoSet), false)]
    ///         [SerializeField, Required] private Settings _settings;
    ///     }
    /// }
    /// </code></example>
    public class ShowWhenAttribute : PropertyAttribute
    {
        public enum Comparer { Equals, Or, Greater, Smaller }
        
        public readonly string   variable;
        public readonly object   value;
        public readonly Comparer comparer;
        public readonly bool     showAsObject;
        
        
        /// <summary>
        /// Shows when the "Variable" field does not match the "value"
        /// </summary>
        public ShowWhenAttribute(string variable, string value, bool showAsObject = true)
        {
            this.variable     = variable;
            this.value        = value;
            this.showAsObject = showAsObject;
        }
        
        /// <summary>
        /// Shows when the "Variable" field does not match the "value"
        /// </summary>
        public ShowWhenAttribute(string variable, int value, Comparer comparer = Comparer.Equals)
        {
            this.variable = variable;
            this.value    = value;
            this.comparer = comparer;
            showAsObject = true;
        }
        
        /// <summary>
        /// Shows when the "Variable" field returns true on tne comparer 
        /// </summary>
        public ShowWhenAttribute(string variable, int value, bool showAsObject, Comparer comparer = Comparer.Equals)
        {
            this.variable     = variable;
            this.value        = value;
            this.comparer     = comparer;
            this.showAsObject = showAsObject;
        }
        
        /// <summary>
        /// Shows when the "Variable" field returns true on tne comparer 
        /// </summary>
        public ShowWhenAttribute(string variable, float value, Comparer comparer = Comparer.Equals)
        {
            this.variable = variable;
            this.value    = value;
            this.comparer = comparer;
            showAsObject = true;
        }
        
        /// <summary>
        /// Shows when the "Variable" field returns true on tne comparer 
        /// </summary>
        public ShowWhenAttribute(string variable, float value, bool showAsObject, Comparer comparer = Comparer.Equals)
        {
            this.variable     = variable;
            this.value        = value;
            this.comparer     = comparer;
            this.showAsObject = showAsObject;
        }
        
        /// <summary>
        /// Shows when the "Variable" field matches the bool value
        /// </summary>
        public ShowWhenAttribute(string variable, bool value, bool showAsObject = true)
        {
            this.variable     = variable;
            this.value        = value;
            this.showAsObject = showAsObject;
        }


        
    }
}
