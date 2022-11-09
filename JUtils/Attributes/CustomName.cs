using System;
using JUtils.Editor;
using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class CustomName : PropertyAttribute
    {
        private readonly string _name;


        public CustomName(string name)
        {
            this._name = name;
        }
        
        
        #if UNITY_EDITOR
        private class NameEditor : JUtilsAttributeEditor
        {
            public override Type targetAttribute { get; } = typeof(CustomName);


            public override void PreFieldDrawn(JUtilsEditorInfo info)
            {
                info.label.text = ((CustomName)info.attribute)._name;
            }
        }
        #endif
    }
}
