using System;
using System.Reflection;
using JetBrains.Annotations;
using JUtils.Editor;
using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class ReadOnly : PropertyAttribute
    {
    }
    
    
#if UNITY_EDITOR

    public class MyClass : JUtilsAttributeEditor
    {
        public override Type targetAttribute { get; } = typeof(ReadOnly);

        public override void PreFieldDrawn(JUtilsEditorInfo info)
        {
            GUI.enabled = false;
        }


        public override void PostFieldDrawn(JUtilsEditorInfo info)
        {
            GUI.enabled = true;
        }
    }
    
#endif
}
