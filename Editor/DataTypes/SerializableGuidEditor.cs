using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(SerializableGuid))]
    public class SerializableGuidEditor : PropertyDrawer
    {
        private const string FIELD_A = "_a";
        private const string FIELD_B = "_b";


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            TextField textField = new(property.displayName);
            textField.RegisterValueChangedCallback(e => HandleValueChanged(property, e.newValue, textField));

            Button randomizeButton = new();
            randomizeButton.clicked += () => HandleRandomizeClicked(property, textField);
            randomizeButton.text = "Randomize";

            root.style.flexDirection = FlexDirection.Row;
            textField.AddToClassList("unity-base-field__aligned");
            textField.labelElement.AddToClassList("unity-property-field__label");
            textField.ElementAt(1).AddToClassList("unity-property-field__input");
            textField.style.flexGrow = 1;
            textField.style.flexShrink = 1;
            randomizeButton.style.flexShrink = 1;
            randomizeButton.style.maxWidth = 128;

            root.Add(textField);
            root.Add(randomizeButton);

            RenderProperty(property, textField);

            return root;
        }


        private void RenderProperty(SerializedProperty property, TextField textField)
        {
            textField.SetValueWithoutNotify(new SerializableGuid(property.FindPropertyRelative(FIELD_A).ulongValue, property.FindPropertyRelative(FIELD_B).ulongValue).ToString());
        }


        private void HandleRandomizeClicked(SerializedProperty property, TextField textField)
        {
            SerializableGuid guid = SerializableGuid.Random();

            property.FindPropertyRelative(FIELD_A).ulongValue = guid.a;
            property.FindPropertyRelative(FIELD_B).ulongValue = guid.b;
            property.serializedObject.ApplyModifiedProperties();

            RenderProperty(property, textField);
        }


        private void HandleValueChanged(SerializedProperty property, string newValue, TextField textField)
        {
            if (!SerializableGuid.TryParse(newValue, out SerializableGuid guid)) {
                Debug.LogError($"Could not parse the entered guid for field '{property.propertyPath}'");
                return;
            }

            property.FindPropertyRelative(FIELD_A).ulongValue = guid.a;
            property.FindPropertyRelative(FIELD_B).ulongValue = guid.b;
            property.serializedObject.ApplyModifiedProperties();

            RenderProperty(property, textField);
        }
    }
}