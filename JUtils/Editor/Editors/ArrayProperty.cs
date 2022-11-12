using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace JUtils.Editor.Editors
{
#if UNITY_EDITOR
    internal class ArrayProperty : JUtilsPropertyDrawer
    {
        public static List<string> _openPaths = new ();


        public override void OnGUI(SerializedProperty property, GUIContent label)
        {
            const int foldoutHeight = 20;
            
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
            
            object obj = JUtilsEditor.GetObjectViaPath(property.propertyPath, property.serializedObject.targetObject as MonoBehaviour);

            if (obj is not IList list) throw new Exception($"Object {property.propertyPath} does not inherit IList");
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
#endif
}
