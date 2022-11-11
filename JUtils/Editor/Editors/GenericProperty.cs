using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;



namespace JUtils.Editor.Editors
{
    internal class GenericProperty : JUtilsPropertyDrawer
    {
        public static List<string> _openPaths = new ();


        public override void OnGUI(SerializedProperty property, GUIContent label)
        {
            string path = property.propertyPath;
            bool isOpen = _openPaths.Contains(path);
            
            //  Foldout knob
            
            bool openPhase = EditorGUILayout.Foldout(isOpen, label, true);
            if (openPhase != isOpen) {
                if (openPhase)
                    _openPaths.Add(path);
                else
                    _openPaths.Remove(path);
            }

            if (!openPhase) return;
            
            //  Displaying

            EditorGUI.indentLevel++;
            int depth = property.depth + 1;
            
            foreach (SerializedProperty child in property) {
                if (depth != child.depth) continue;
                JUtilsEditor.PropertyField(child);
            }
            EditorGUI.indentLevel--;
        }


        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            const int foldoutHeight = 20;
            
            string path = property.propertyPath;
            bool isOpen = _openPaths.Contains(path);
            
            //  Foldout knob

            rect.height = foldoutHeight;
            
            bool openPhase = EditorGUI.Foldout(rect, isOpen, label, true);
            if (openPhase != isOpen) {
                if (openPhase)
                    _openPaths.Add(path);
                else
                    _openPaths.Remove(path);
            }

            if (!openPhase) return;
            
            //  Displaying

            EditorGUI.indentLevel++;
            int depth = property.depth + 1;
            
            foreach (SerializedProperty child in property) {
                if (depth != child.depth) continue;
                JUtilsEditor.PropertyField(child);
            }

            EditorGUI.indentLevel--;
        }
    }
}
