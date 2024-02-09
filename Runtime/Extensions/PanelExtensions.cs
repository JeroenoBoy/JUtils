using UnityEngine.UIElements;

namespace JUtils
{
    public static class PanelExtensions
    {
        /// <summary>
        /// Check if the panel is only in runtime
        /// </summary>
        public static bool IsRuntimePanel(this IPanel self)
        {
            return self.GetType().Name == "RuntimePanel";
        }
    }
}