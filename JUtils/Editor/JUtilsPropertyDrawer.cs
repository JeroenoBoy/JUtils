using UnityEditor;
using UnityEngine;



namespace JUtils.Editor
{
    internal abstract class JUtilsPropertyDrawer
    {
        public abstract void OnGUI(SerializedProperty property, GUIContent label);
        public abstract void OnGUI(Rect rect, SerializedProperty property, GUIContent label);
    }
}
