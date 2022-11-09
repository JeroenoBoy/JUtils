using System;
using System.Linq;
using JUtils.Editor;
using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class ShowWhen : PropertyAttribute
    {
        public enum Comparer { Equals, Or, Greater, Smaller }
        
        private string   _variable;
        private object   _value;
        private Comparer _comparer;
        
        
        /// <summary>
        /// Serializes an attribute when the given variable is equal to the given value
        /// </summary>
        public ShowWhen(string variable, string value)
        {
            _variable     = variable;
            _value        = value;
        }
        
        
        /// <summary>
        /// Serializes an attribute when the given variable is equal to the given value
        /// </summary>
        public ShowWhen(string variable, int value, Comparer comparer = Comparer.Equals)
        {
            _variable = variable;
            _value    = value;
            _comparer = comparer;
        }
        
        
        /// <summary>
        /// Serializes an attribute when the given variable is equal to the given value
        /// </summary>
        public ShowWhen(string variable, float value, Comparer comparer = Comparer.Equals)
        {
            _variable = variable;
            _value    = value;
            _comparer = comparer;
        }


        #region Obselete

        /// <summary>
        /// Serializes an attribute when the given variable is equal to the given value
        /// </summary
        [Obsolete("showAsObject is obsolete. Add the Unpack attribute instead")]
        public ShowWhen(string variable, int value, bool showAsObject, Comparer comparer = Comparer.Equals)
        {
            _variable     = variable;
            _value        = value;
            _comparer     = comparer;
        }
        
        
        /// <summary>
        /// Serializes an attribute when the given variable is equal to the given value
        /// </summary>
        [Obsolete("showAsObject is obsolete. Add Unpack instead")]
        public ShowWhen(string variable, string value, bool showAsObject)
        {
            _variable     = variable;
            _value        = value;
        }
        
        
        /// <summary>
        /// Serializes an attribute when the given variable is equal to the given value
        /// </summary>
        [Obsolete("showAsObject is obsolete. Add the Unpack attribute instead")]
        public ShowWhen(string variable, float value, bool showAsObject, Comparer comparer = Comparer.Equals)
        {
            _variable     = variable;
            _value        = value;
            _comparer     = comparer;
        }
        
        
        /// <summary>
        /// Serializes an attribute when the given variable is equal to the given value
        /// </summary>
        [Obsolete("showAsObject is obsolete. Add the Unpack attribute instead")]
        public ShowWhen(string variable, bool value, bool showAsObject)
        {
            _variable     = variable;
            _value        = value;
        }
        
        
        /// <summary>
        /// Serializes an attribute when the given variable is equal to the given value
        /// </summary>
        public ShowWhen(string variable, bool value)
        {
            _variable     = variable;
            _value        = value;
        }

        #endregion



#if UNITY_EDITOR
        private class SerializeWhenEditor : JUtilsAttributeEditor
        {
            public override Type targetAttribute { get; } = typeof(ShowWhen);
            private bool _includeChildren = true;


            public override void PreFieldDrawn(JUtilsEditorInfo info)
            {
                JUtilsEditor.hidden = !Matches(info.property, info.attribute as ShowWhen);
            }


            public override void PostFieldDrawn(JUtilsEditorInfo info)
            {
                JUtilsEditor.hidden = false;
            }
            

            public bool Matches(SerializedProperty property, ShowWhen attribute)
            {
                //  Getting the variable & how far back we need to go
                
                string[] variables = attribute._variable.Split('.');
                string nane       = variables.Last();
                
                int backTrace = variables.Length;

                //  Getting the value
                
                string[] paths = property.propertyPath.Split('.');
                string    path = paths.Length > backTrace
                    ? string.Join('.', paths[..^backTrace]) + '.' + nane
                    : nane;
                
                SerializedProperty variable = property.serializedObject.FindProperty(path);
                
                //  Checking if the variable is null
                
                if (variable == null) {
                    Debug.LogError("Variable " + path + " not found");
                    return false;
                }
                
                //  Handling the comparison

                return attribute._value switch {
                    string value => variable.stringValue == value,
                    bool   value => variable.boolValue   == value,
                    
                    int value when attribute._comparer == Comparer.Equals  => variable.intValue == value,
                    int value when attribute._comparer == Comparer.Greater => variable.intValue > value,
                    int value when attribute._comparer == Comparer.Smaller => variable.intValue < value,
                    int value when attribute._comparer == Comparer.Or      => (variable.intValue & value) != 0,
                    
                    float value when attribute._comparer == Comparer.Equals  => variable.floatValue == value,
                    float value when attribute._comparer == Comparer.Greater => variable.floatValue > value,
                    float value when attribute._comparer == Comparer.Smaller => variable.floatValue < value,
                    float       when attribute._comparer == Comparer.Or      => throw new Exception("Or is now allowed on floats"),
                    
                    _ => throw new Exception("Unsupported value type")
                };
            }
        }
#endif
    }
}
