using System;
using System.Collections;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Comparer = JUtils.ShowWhenAttribute.Comparer;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(ShowWhenAttribute))]
    public class ShowWhenEditor : PropertyDrawer
    {
        protected new ShowWhenAttribute attribute => (ShowWhenAttribute)base.attribute;
        
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField propertyField = new(property);

            EditorCoroutine routine = EditorCoroutineUtility.StartCoroutine(MatchRoutine(propertyField, property), property);
            propertyField.RegisterCallback((DetachFromPanelEvent _) => EditorCoroutineUtility.StopCoroutine(routine));

            return propertyField;
        }

        
        private IEnumerator MatchRoutine(VisualElement element, SerializedProperty property)
        {
            while (true) {
                element.style.display = Matches(property) ? DisplayStyle.Flex : DisplayStyle.None;
                yield return new WaitForSeconds(1);
            }
        }


        public bool Matches(SerializedProperty property)
        {
            string[] variables = attribute.variable.Split('.');
            string nane = variables.Last();
            
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