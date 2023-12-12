using JUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(TypeSelectorAttribute))]
    public class TypeSelectorEditor : PropertyDrawer
    {
        private static readonly Dictionary<string, Type[]> _map = new ();
        
        public TypeSelectorAttribute target => attribute as TypeSelectorAttribute;


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Type[] availableTypes = GetAvailableTypes(property.managedReferenceFieldTypename);
            VisualElement root = new();
            PropertyField propertyField = new(property, property.displayName);
            Label emptyLabel = new(property.displayName);
            DropdownField dropdownField = new() {
                style = { position = Position.Absolute, right = 0 },
                value = string.IsNullOrEmpty(property.managedReferenceFullTypename) ? "Unset" : AssemblyJUtils.GetType(property.managedReferenceFullTypename).Name,
                choices = availableTypes.Select(it => it.Name).Prepend("Unset").ToList()
            };

            dropdownField.RegisterValueChangedCallback(e => {
                Type newType = availableTypes.FirstOrDefault(it => it.Name == e.newValue);
                if (newType == null) {
                    property.managedReferenceValue = null;
                } else if (e.newValue != e.previousValue) {
                    property.managedReferenceValue = Activator.CreateInstance(newType);
                }

                property.serializedObject.ApplyModifiedProperties();
                ChangeEnabled();
            });
            
            ChangeEnabled();
            
            root.Add(propertyField);
            root.Add(emptyLabel);
            root.Add(dropdownField);

            void ChangeEnabled()
            {
                bool isNull = property.managedReferenceValue == null;
                propertyField.style.display = isNull ? DisplayStyle.None : DisplayStyle.Flex;
                emptyLabel.style.display = isNull ? DisplayStyle.Flex : DisplayStyle.None;
                propertyField.BindProperty(property);
            }
            
            return root;
        }


        /// <summary>
        /// Gets the context of this property
        /// </summary>
        private Type[] GetAvailableTypes(string typeName)
        {
            if (_map.TryGetValue(typeName, out Type[] availableTypes)) return availableTypes;

            Type targetType = AssemblyJUtils.GetType(typeName);
            _map[typeName] = AssemblyJUtils.GetAllTypes()
                .Where(t => AssemblyJUtils.ExtendsClassOrInterface(t, targetType) && !t.IsSubclassOf(typeof(Object)))
                .ToArray();
            
            return _map[typeName];
        }
    }
}