using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JUtils.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor.IMGUI.Controls;
#endif



namespace JUtils.Attributes
{
    public class TypeSelector : PropertyAttribute
    {
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(TypeSelector))]
        private class PopulatorEditor : PropertyDrawer
        {
            private static Dictionary<string, Context> _map = new ();
            
            
            public TypeSelector target => attribute as TypeSelector;
            

            /// <summary>
            /// Gets the context of this property
            /// </summary>
            private Context GetContext(string path, string targetType, string currentType)
            {
                if (_map.ContainsKey(path)) return _map[path];

                Type target  = JUtility.GetType(targetType);
                Type current = currentType == "" ? null : JUtility.GetType(currentType);
                
                Context ctx = new (target, current);
                _map[path] = ctx;
                return ctx;
            }

            
            //  Standard overriden properties for PropertyDrawer
            
            
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                
                Context ctx = GetContext(property.propertyPath, property.managedReferenceFieldTypename, property.managedReferenceFullTypename);
                
                return ctx.dropDown.ItemIsNull() ? 18 : EditorGUI.GetPropertyHeight(property);
            }


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                Context ctx = GetContext(property.propertyPath, property.managedReferenceFieldTypename, property.managedReferenceFullTypename);
                
                //  Getting the current reference and checking if the property has a SerializeReference Attribute
                
                object o;
                try {
                    o = property.managedReferenceValue;
                }
                catch {
                    Debug.LogError("IPopulator requires \"SerializeReference\" attribute");
                    return;
                }

                EditorGUI.BeginProperty(position, label, property);
                
                //  Getting dropdown height

                Rect rect = new (position)
                {
                    x = EditorGUIUtility.labelWidth + position.x + 1,
                    height = 18,
                    width = position.width - EditorGUIUtility.labelWidth
                };
                
                //  Drawing the dropdown

                GUIContent ddLabel = new (ctx.dropDown.ItemName());
                if (EditorGUI.DropdownButton(rect, ddLabel, FocusType.Keyboard)) {
                    ctx.dropDown.Show(new Rect(rect) {height = 0});
                }
                
                //  Setting value

                if (o is null || o.GetType() != ctx.dropDown.ItemType()) {
                    if (ctx.dropDown.ItemIsNull())
                        property.managedReferenceValue = null;
                    else
                        property.managedReferenceValue = Activator.CreateInstance(ctx.dropDown.ItemType());
                }
                
                //  Property

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
                /// <param name="target">The base type to target</param>
                public Context(Type target, Type currentType)
                {
                    types = JUtility.GetAllTypes()
                        .Where(t => JUtility.ExtendsClassOrInterface(t, target) && !t.IsSubclassOf(typeof(Object)))
                        .ToArray();
    
                    dropDown = new DropDown(types, currentType);
                }
            }
            
            
            
            private class DropDown : AdvancedDropdown
            {
                private Type[] _types;
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
                    if (item.name == "Unset")
                        _selectedType = null;
                    else
                        _selectedType = _types.First(t => t.Name == item.name);
                }
                
                
                //  Easy accessible functions


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
#endif
    }
}