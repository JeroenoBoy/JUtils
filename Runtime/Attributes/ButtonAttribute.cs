using System;
using JetBrains.Annotations;

namespace JUtils
{
    /// <summary>
    /// Attach this button to a method to make it clickable in the inspector. This attribute will also draw parameters if it can.
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class ButtonExample : MonoBehaviour
    ///     {
    ///         [Button]
    ///         public void SimpleButton() {
    ///             Debug.Log("The Simple button has been pressed");
    ///         }
    ///     }
    /// }
    /// </code></example>
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        [CanBeNull] public readonly string name;
        public bool clickableInEditor;
        
        
        /// <summary>
        /// Create an inspector button for this atribute
        /// </summary>
        /// <param name="name">The name of the button</param>
        /// <param name="clickableInEditor">Should the button only run in playmode</param>
        public ButtonAttribute(
            [CanBeNull] string name = null,
            bool clickableInEditor = false)
        {
            this.name = name;
            this.clickableInEditor = clickableInEditor;
        }
    }
}
