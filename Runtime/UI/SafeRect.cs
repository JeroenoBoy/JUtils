using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils.UI
{
    [UxmlElement]
    public partial class SafeRect : VisualElement
    {
        public SafeRect()
        {
            style.width = new Length(100f, LengthUnit.Percent);
            style.height = new Length(100f, LengthUnit.Percent);

            RegisterCallback<AttachToPanelEvent>(HandleAttachedToPanel);
        }


        private void HandleAttachedToPanel(AttachToPanelEvent evt)
        {
            Rect safeArea = Screen.safeArea;
            Rect currentRect = worldBound;

            if (!Application.isPlaying) return;
            if (safeArea.Contains(new Vector2(currentRect.xMin, currentRect.yMin)) && safeArea.Contains(new Vector2(currentRect.xMax, currentRect.yMax))) return;

            style.top = new Length(safeArea.yMin - currentRect.yMin, LengthUnit.Pixel);
            style.left = new Length(safeArea.xMin - currentRect.xMin, LengthUnit.Pixel);

            // style.right = new Length(safeArea.xMax - newRect.xMax, LengthUnit.Pixel);
            // style.bottom = new Length(safeArea.yMax - newRect.yMax, LengthUnit.Pixel);
        }
    }
}