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

            public override bool OverrideFieldDraw(JUtilsEditorInfo info)
            {
                EditorGUILayout.PropertyField(info.property);

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
        private bool _isUnityObject;
        [CanBeNull]
        private string _relativePath;


        public ExperimentalRequired() { }
        public ExperimentalRequired(bool isUnityObject = true, [CanBeNull] string relativePath = null)
        {
            _isUnityObject = isUnityObject;
            _relativePath = relativePath;
        }


#if UNITY_EDITOR

        public class RequiredEditor : JUtilsAttributeEditor
        {
            public override Type targetAttribute { get; } = typeof(ExperimentalRequired);
        }
#endif
    }
}
