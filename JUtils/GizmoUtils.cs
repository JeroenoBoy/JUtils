using System;
using UnityEditor;
using UnityEngine;



namespace JUtils
{
    public static class GizmoUtils
    {
        /// <summary>
        /// Draws a ray with a disc relative to the position and the input ray
        /// </summary>
        public static void DrawVector(Vector3 position, Vector3 Vector, Color color)
        {
#if UNITY_EDITOR
            Vector3 target = position + Vector;
            Handles.color = color;
            Handles.DrawLine(position, target);
            Handles.DrawSolidDisc(target, Vector3.up, 0.3f);
#endif
        }
        
        
        /// <summary>
        /// Draw text at a point in the world
        /// </summary>
        /// <param name="text">The text to draw</param>
        /// <param name="worldPos">The position to draw the text</param>">
        /// <param name="colour">The color to draw</param>">
        /// https://gist.github.com/Arakade/9dd844c2f9c10e97e3d0
        public static void DrawString(string text, Vector3 worldPos, Color? colour = null)
        {
#if UNITY_EDITOR
            Handles.BeginGUI();
            try {
                if (colour.HasValue) GUI.color = colour.Value;
                
                SceneView view      = SceneView.currentDrawingSceneView;
                Vector3   screenPos = view.camera.WorldToScreenPoint(worldPos);
                Vector2   size      = GUI.skin.label.CalcSize(new GUIContent(text));
                
                GUI.Label(new Rect(screenPos.x - size.x / 2, -screenPos.y + view.position.height + 4, size.x, size.y), text);
            }
            catch (Exception _) { }
            Handles.EndGUI();
#endif
        }
    }
}