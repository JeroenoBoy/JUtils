using UnityEditor;
using UnityEngine;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(Weighted<>))]
    public class WeightedEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value"), label, true);
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty weightField = property.FindPropertyRelative("_weight");
            SerializedProperty valueField  = property.FindPropertyRelative("_value");
            
            if (valueField.propertyType == SerializedPropertyType.Generic)
                HandleManaged(position, label, weightField, valueField);
            else
                HandleUnmanaged(position, label, weightField, valueField);
            
            property.serializedObject.ApplyModifiedProperties();
        }


        private void HandleManaged(Rect position, GUIContent label, SerializedProperty weightField, SerializedProperty valueField)
        {
            GUIContent labelCopy = new(label);
            
            Rect weightFieldRect = new(position) {x = position.x + position.width - 48, width = 48, height = EditorGUI.GetPropertyHeight(weightField)};
            Rect valueFieldRect  = new(position) {width = position.width - 50};
            
            EditorGUI.PropertyField(valueFieldRect, valueField, labelCopy, true);
            EditorGUI.PropertyField(weightFieldRect, weightField, GUIContent.none);
        }


        private void HandleUnmanaged(Rect position, GUIContent label, SerializedProperty weightField, SerializedProperty valueField)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            
            Rect labelRect = new (position) {width = labelWidth};
            EditorGUI.LabelField(labelRect, label);
            
            float valueWidth = 48;
            
            Rect weightedFieldRect = new (position) { x = position.x + labelWidth, width =  valueWidth, height = EditorGUI.GetPropertyHeight(weightField)};
            weightField.floatValue = EditorGUI.FloatField(weightedFieldRect, weightField.floatValue);
            
            Rect valueFieldRect = new (position) {x = position.x + labelWidth + valueWidth + 2, width = position.width - labelWidth - valueWidth - 2};
            EditorGUI.PropertyField(valueFieldRect, valueField, GUIContent.none);
        }
    }
}