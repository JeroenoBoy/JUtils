﻿using System;
using System.Linq;
using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class ShowWhen : PropertyAttribute
    {
        public enum Comparer { EQUALS, OR, GREATER, LESSER }
        
        private string   _variable;
        private object   _value;
        private Comparer _comparer;
        private bool     _showAsObject = true;
        
        /**
         * Serializes an attribute when the given variable is equal to the given value
         */
        public ShowWhen(string variable, string value)
        {
            _variable = variable;
            _value    = value;
        }
        
        /**
         * Serializes an attribute when the given variable is equal to the given value
         */
        public ShowWhen(string variable, string value, bool showAsObject)
        {
            _variable     = variable;
            _value        = value;
            _showAsObject = showAsObject;
        }
        
        /**
         * Serializes an attribute when the given variable is equal to the given value
         */
        public ShowWhen(string variable, int value, Comparer comparer = Comparer.EQUALS)
        {
            _variable = variable;
            _value    = value;
            _comparer = comparer;
        }
        
        /**
         * Serializes an attribute when the given variable is equal to the given value
         */
        public ShowWhen(string variable, int value, bool showAsObject, Comparer comparer = Comparer.EQUALS)
        {
            _variable     = variable;
            _value        = value;
            _comparer     = comparer;
            _showAsObject = showAsObject;
        }
        
        /**
         * Serializes an attribute when the given variable is equal to the given value
         */
        public ShowWhen(string variable, bool value, bool showAsObject = true)
        {
            _variable     = variable;
            _value        = value;
            _showAsObject = showAsObject;
        }


        
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ShowWhen))]
        private class SerializeWhenEditor : PropertyDrawer
        {
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
                
                var asObject = ((ShowWhen) attribute)._showAsObject;

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
                var attribute = (ShowWhen)this.attribute;
                
                //  Getting the variable & how far back we need to go
                
                var variables = attribute._variable.Split('.');
                var nane       = variables.Last();
                
                int backTrace = variables.Length;

                //  Getting the value
                
                var paths = property.propertyPath.Split('.');
                var path   = paths.Length > backTrace
                    ? string.Join('.', paths[..^backTrace]) + '.' + nane
                    : nane;
                
                var variable = property.serializedObject.FindProperty(path);
                
                //  Checking if the variable is null
                
                if (variable == null) {
                    Debug.LogError("Variable " + path + " not found");
                    return false;
                }
                
                //  Handling the comparison

                return attribute._value switch {
                    string value => variable.stringValue == value,
                    bool   value => variable.boolValue   == value,
                    
                    int value when attribute._comparer == Comparer.EQUALS  => variable.intValue == value,
                    int value when attribute._comparer == Comparer.GREATER => variable.intValue > value,
                    int value when attribute._comparer == Comparer.LESSER  => variable.intValue < value,
                    int value when attribute._comparer == Comparer.OR      => (variable.intValue & value) != 0,
                    
                    _ => throw new Exception("Unsupported value type")
                };
            }
        }
#endif
    }
}
