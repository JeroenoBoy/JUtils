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
            string path = property.propertyPath;
            bool isOpen = _openPaths.Contains(path);
            
            //  Getting list

            float width = Screen.width;

            //  Foldout knob

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginHorizontal(new GUIStyle() {fixedWidth = width - 64 - 7, fixedHeight = GUI.skin.textField.fixedHeight});
            bool openPhase = EditorGUILayout.Foldout(isOpen, label, true);
            EditorGUILayout.EndHorizontal();
            if (openPhase != isOpen) {
                if (openPhase)
                    _openPaths.Add(path);
                else
                    _openPaths.Remove(path);
            }

            int oldIndex = property.arraySize;
            property.arraySize = EditorGUILayout.IntField(oldIndex, new GUIStyle(GUI.skin.textField) {fixedWidth = 48, margin = new RectOffset(0, 0, -2, 0)});
            EditorGUILayout.EndHorizontal();
            
            if (!openPhase) return;
            
            //  Showing children

            EditorGUILayout.BeginVertical(GUI.skin.scrollView);
            
            for (int i = 0; i < oldIndex; i++) {
                SerializedProperty arrayProperty = property.GetArrayElementAtIndex(i);
                JUtilsEditor.PropertyField(arrayProperty);
            }
            
            EditorGUILayout.EndVertical();
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
