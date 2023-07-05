using UnityEngine.UIElements;



namespace JUtils.UI
{
    /// <summary>
    /// A class containing useful extensions for visual elements
    /// </summary>
    public static class UIExtensions
    {
        /// <summary>
        /// Sets the element's display property based on the enabled bool
        /// </summary>
        public static void SetDisplay(this VisualElement element, bool show)
        {
            element.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }


        /// <summary>
        /// Returns true if the visual element's display property is set to flex
        /// </summary>
        public static bool IsShowing(this VisualElement element)
        {
            return element.style.display == DisplayStyle.Flex;
        }
    }
}
