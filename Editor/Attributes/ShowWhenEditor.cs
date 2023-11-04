using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Comparer = JUtils.ShowWhenAttribute.Comparer;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(ShowWhenAttribute))]
    public class SerializeWhenEditor : PropertyDrawer
    {
        protected new ShowWhenAttribute attribute => (ShowWhenAttribute)base.attribute;
        private bool _includeChildren = true;
        
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return -2;
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!Matches(property)) return;

            if (property.propertyType != SerializedPropertyType.Generic) {
                EditorGUILayout.PropertyField(property, label);
                return;
            }
            
            //  Drawing the property
            
            bool asObject = attribute.showAsObject;

            //  Skipping the property if we don't want to show it as an object
            if (asObject) {
                EditorGUILayout.BeginHorizontal();
                _includeChildren = EditorGUILayout.Foldout(_includeChildren, label);
                EditorGUILayout.EndHorizontal();

                if (!_includeChildren) return;
                EditorGUI.indentLevel++;
            }
            
            //  Drawing the children
            
            property.NextVisible(true);
            int depth = property.depth;

            do EditorGUILayout.PropertyField(property);
            while (property.NextVisible(false) && property.depth == depth);

            //  Decreasing the indent level
            if (asObject) EditorGUI.indentLevel--;
        }


        public bool Matches(SerializedProperty property)
        {
            string[] variables = attribute.variable.Split('.');
            string nane       = variables.Last();
            
            int backTrace = variables.Length;

            string[] paths = property.propertyPath.Split('.');
            string    path = paths.Length > backTrace
                ? string.Join('.', paths[..^backTrace]) + '.' + nane
                : nane;
            
            SerializedProperty variable = property.serializedObject.FindProperty(path);
            
            if (variable == null) {
                Debug.LogError("Variable " + path + " not found");
                return false;
            }
            
            return attribute.value switch {
                string value => variable.stringValue == value,
                bool   value => variable.boolValue   == value,
                
                int value when attribute.comparer == Comparer.Equals  => variable.intValue == value,
                int value when attribute.comparer == Comparer.Greater => variable.intValue > value,
                int value when attribute.comparer == Comparer.Smaller => variable.intValue < value,
                int value when attribute.comparer == Comparer.Or      => (variable.intValue & value) != 0,
                
                float value when attribute.comparer == Comparer.Equals  => Mathf.Approximately(variable.floatValue, value),
                float value when attribute.comparer == Comparer.Greater => variable.floatValue > value,
                float value when attribute.comparer == Comparer.Smaller => variable.floatValue < value,
                float       when attribute.comparer == Comparer.Or      => throw new Exception("Or is now allowed on floats"),
                
                _ => throw new Exception("Unsupported value type")
            };
        }
    }
}