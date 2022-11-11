using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using JUtils.Editor;
using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class Required : PropertyAttribute
    {
#if UNITY_EDITOR

        private class RequiredEditor : JUtilsAttributeEditor
        {
            public override Type targetAttribute { get; } = typeof(Required);

            public override bool OverrideFieldDraw(JUtilsEditorInfo info, GUIContent label)
            {
                EditorGUILayout.PropertyField(info.property, label);

                if (info.property.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox($"Field \"{info.property.displayName}\" is required", MessageType.Error);
                    EditorGUILayout.Space(4);
                }
                
                return true;
            }
        }
#endif
    }
    
    
    
    public class ExperimentalRequired : PropertyAttribute
    {
        [CanBeNull] private string _relativePath = null;


        public ExperimentalRequired() { }
        public ExperimentalRequired(string relativePath = null)
        {
            _relativePath = relativePath;
        }


#if UNITY_EDITOR

        public class RequiredEditor : JUtilsAttributeEditor
        {
            public override Type targetAttribute { get; } = typeof(ExperimentalRequired);

            
            public override void PostFieldDrawn(JUtilsEditorInfo info)
            {
                ExperimentalRequired attribute = info.attribute as ExperimentalRequired;
                
                bool show = false;
                
                if (attribute._relativePath == null) {
                    show = info.field.GetValue(info.parentObject).Equals(null);
                }
                else {
                    Type type = info.currentObject.GetType();
                    FieldInfo field = type.GetField(attribute._relativePath, JUtilsEditor.fieldBindings);
                    if (field == null) throw new Exception($"Field \"{attribute._relativePath}\" was not found");
                    show = field.GetValue(info.currentObject).Equals(null);
                }

                if (!show) return;
                
                EditorGUILayout.HelpBox($"Field \"{info.property.displayName}\" is required", MessageType.Error);
                EditorGUILayout.Space(4);
            }
        }
#endif
    }
}
