using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredEditor : PropertyDrawer
    {
        private HelpBox _helpBox;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            
            PropertyField field = new(property);
            _helpBox = new HelpBox();
            _helpBox.messageType = HelpBoxMessageType.Error;
            
            field.RegisterValueChangeCallback(changeEvent => {
                UpdateText(changeEvent.changedProperty);
            });
            
            root.Add(field);
            root.Add(_helpBox);
            
            UpdateText(property);
            return root;
        }

        private void UpdateText(SerializedProperty property)
        {
            if (!property.type.StartsWith("PPtr<")) {
                _helpBox.text = "Value must be an object reference type";
            } else if (property.objectReferenceValue == null) {
                _helpBox.text = $"Field \"{property.displayName}\" cannot be null";
            } else {
                _helpBox.text = "";
            }

            _helpBox.style.display = string.IsNullOrEmpty(_helpBox.text) ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}