using UnityEditor;
using UnityEngine;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(MinMax))]
    public class MinMaxEditor : PropertyDrawer
    {
        private static readonly GUIContent[] _labels = {
            new("Min"),
            new("Max")
        };

        private readonly float[] _values = new float[2];
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty minField = property.FindPropertyRelative(nameof(MinMax._min));
            SerializedProperty maxField = property.FindPropertyRelative(nameof(MinMax._max));

            bool mode = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = false;
            
            EditorGUI.LabelField(position, label);
            
            _values[0] = minField.floatValue;
            _values[1] = maxField.floatValue;

            Rect rect = new(position) { x = position.x + EditorGUIUtility.labelWidth + 2, width = position.width - EditorGUIUtility.labelWidth - 2};
            EditorGUI.MultiFloatField(rect, _labels, _values);

            minField.floatValue = _values[0];
            maxField.floatValue = _values[1];

            EditorGUIUtility.wideMode = mode;
            
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}