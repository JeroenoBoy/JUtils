using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(MinMax))]
    public class MinMaxEditor : PropertyDrawer
    {
        public override bool CanCacheInspectorGUI(SerializedProperty property) => true;

        private static readonly GUIContent[] _labels = {
            new("Min"),
            new("Max")
        };

        private readonly float[] _values = new float[2];

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty minField = property.FindPropertyRelative("_min");
            SerializedProperty maxField = property.FindPropertyRelative("_max");

            VisualElement root = new();
            
            root.Add(new TextField(property.name));
            
            return root;
        }

        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        // {
        //
        //     bool mode = EditorGUIUtility.wideMode;
        //     EditorGUIUtility.wideMode = false;
        //     
        //     EditorGUI.LabelField(position, label);
        //     
        //     _values[0] = minField.floatValue;
        //     _values[1] = maxField.floatValue;
        //
        //     Rect rect = new(position) { x = position.x + EditorGUIUtility.labelWidth + 2, width = position.width - EditorGUIUtility.labelWidth - 2};
        //     EditorGUI.MultiFloatField(rect, _labels, _values);
        //
        //     minField.floatValue = _values[0];
        //     maxField.floatValue = _values[1];
        //
        //     EditorGUIUtility.wideMode = mode;
        //     
        //     property.serializedObject.ApplyModifiedProperties();
        // }
    }
}