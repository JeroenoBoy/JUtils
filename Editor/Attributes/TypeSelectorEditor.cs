using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(TypeSelectorAttribute))]
    public class TypeSelectorEditor : PropertyDrawer
    {
        private static readonly Dictionary<string, Context> _map = new ();
        
        
        public TypeSelectorAttribute target => attribute as TypeSelectorAttribute;
        

        /// <summary>
        /// Gets the context of this property
        /// </summary>
        private Context GetContext(string path, string targetType, string currentType)
        {
            if (_map.TryGetValue(path, out Context context)) return context;

            Type target  = AssemblyJUtils.GetType(targetType);
            Type current = currentType == "" ? null : AssemblyJUtils.GetType(currentType);
            
            Context ctx = new (target, current);
            _map[path] = ctx;
            return ctx;
        }

        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Context ctx = GetContext(property.propertyPath, property.managedReferenceFieldTypename, property.managedReferenceFullTypename);
            return ctx.dropDown.ItemIsNull() ? 18 : EditorGUI.GetPropertyHeight(property);
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Context ctx = GetContext(property.propertyPath, property.managedReferenceFieldTypename, property.managedReferenceFullTypename);
            
            object o;
            try {
                o = property.managedReferenceValue;
            }
            catch {
                Debug.LogError("IPopulator requires \"SerializeReference\" attribute");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);
            
            Rect rect = new (position)
            {
                x = EditorGUIUtility.labelWidth + position.x + 1,
                height = 18,
                width = position.width - EditorGUIUtility.labelWidth
            };
            
            GUIContent ddLabel = new (ctx.dropDown.ItemName());
            if (EditorGUI.DropdownButton(rect, ddLabel, FocusType.Keyboard)) {
                ctx.dropDown.Show(new Rect(rect) {height = 0});
            }
            
            if (o is null || o.GetType() != ctx.dropDown.ItemType()) {
                property.managedReferenceValue = !ctx.dropDown.ItemIsNull()
                    ? Activator.CreateInstance(ctx.dropDown.ItemType())
                    : null;
            }
            
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }
        
        
        
        private class Context
        {
            public readonly Type[] types;
            public readonly DropDown dropDown;
            
            
            /// <summary>
            /// Select all the types
            /// </summary>
            public Context(Type target, Type currentType)
            {
                types = AssemblyJUtils.GetAllTypes()
                    .Where(t => AssemblyJUtils.ExtendsClassOrInterface(t, target) && !t.IsSubclassOf(typeof(Object)))
                    .ToArray();

                dropDown = new DropDown(types, currentType);
            }
        }
        
        
        
        private class DropDown : AdvancedDropdown
        {
            private readonly Type[] _types;
            private Type _selectedType;
            
            
            public DropDown(Type[] types, Type selectedType) : base(new AdvancedDropdownState())
            {
                _selectedType = selectedType;
                _types = types;
            }


            protected override AdvancedDropdownItem BuildRoot()
            {
                AdvancedDropdownItem root = new ("Scripts");
                
                root.AddChild(new AdvancedDropdownItem("Unset"));
                
                foreach (Type type in _types) {
                    root.AddChild(new AdvancedDropdownItem(type.Name));
                }

                return root;
            }


            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                _selectedType = item.name == "Unset" ? null : _types.First(t => t.Name == item.name);
            }
            
            
            public string ItemName()
            {
                return _selectedType == null ? "Unset" : _selectedType.Name;
            }


            public Type ItemType()
            {
                return _selectedType;
            }


            public bool ItemIsNull()
            {
                return _selectedType == null;
            }
        }
    }
}