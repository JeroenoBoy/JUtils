using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    public abstract class BaseMinMaxEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty minProperty = property.FindPropertyRelative("_min");
            SerializedProperty maxProperty = property.FindPropertyRelative("_max");

            VisualElement root = new();

            float labelWidth = EditorGUIUtility.labelWidth;
            Label label = new(property.displayName);
            
            BindableElement minField = CreateElement("min");
            BindableElement maxField = CreateElement("max");

            minField.BindProperty(minProperty);
            maxField.BindProperty(maxProperty);

            minField.style.paddingLeft = 5;
            minField.style.flexGrow = 1;
            maxField.style.paddingLeft = 10;
            maxField.style.flexGrow = 1;
            minField.style.width = new Length(.5f, LengthUnit.Percent);
            maxField.style.width = new Length(.5f, LengthUnit.Percent);

            Label minLabel = minField.Q<Label>();
            minLabel.style.width = 25;
            minLabel.style.minWidth = 25;
            
            Label maxLabel = maxField.Q<Label>();
            maxLabel.style.width = 28;
            maxLabel.style.minWidth = 28;

            label.style.width = labelWidth;
            label.style.minWidth = labelWidth;
            
            root.style.flexDirection = FlexDirection.Row;
            root.Add(label);
            root.Add(minField);
            root.Add(maxField);
            
            return root;
        }


        protected abstract BindableElement CreateElement(string name);
    }
}