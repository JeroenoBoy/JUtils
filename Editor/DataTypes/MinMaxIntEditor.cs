using UnityEditor;
using UnityEngine;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxInt))]
    public class MinMaxIntEditor : PropertyDrawer
    {
        private static readonly GUIContent[] _labels = {
            new("Min"),
            new("Max")
        };

        private readonly int[] _values = new int[2];
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty minField = property.FindPropertyRelative("_min");
            SerializedProperty maxField = property.FindPropertyRelative("_max");

            bool mode = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = false;
            
            EditorGUI.LabelField(position, label);
            
            _values[0] = minField.intValue;
            _values[1] = maxField.intValue;

            Rect rect = new(position) { x = position.x + EditorGUIUtility.labelWidth + 2, width = position.width - EditorGUIUtility.labelWidth - 2};
            EditorGUI.MultiIntField(rect, _labels, _values);

            minField.intValue = _values[0];
            maxField.intValue = _values[1];

            EditorGUIUtility.wideMode = mode;
            
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}