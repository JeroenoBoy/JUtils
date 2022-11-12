
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JUtils.Attributes;
using JUtils.Editor.Editors;
using UnityEditor;
using UnityEngine;



namespace JUtils.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public sealed class JUtilsEditor : UnityEditor.Editor
    {
        #region Static properties

        #region Caching

        private static bool _loaded;
        private static JUtilsAttributeEditor[] _attributeEditors;
        private static string[] _excludeTypes;
        private static string[] _excludeAttributeTypes;
        public static readonly BindingFlags fieldBindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static GenericProperty _generic;
        
        #endregion


        #region PropertyField
        
        private static JUtilsEditorContext _context;


        /// <summary>
        /// Draw a new PropertyField using the JUtilsEditor
        /// </summary>
        public static void PropertyField(SerializedProperty field, GUIContent label = null)
        {
            if (_context == null) throw new Exception("Cannot call JUtils.PropertyField outside of JUtils editor");

            MonoBehaviour target = _context.target;
            GetObjectViaPath(field.propertyPath, target, out object currentObject);
            
            HandlePropertyField(currentObject.GetType(), target, currentObject, field, label);
        }
        
        
        /// <summary>
        /// Draw a new PropertyField using the JUtilsEditor
        /// </summary>
        public static void PropertyField(Rect rect, SerializedProperty field, GUIContent label = null)
        {
            if (_context == null) throw new Exception("Cannot call JUtils.PropertyField outside of JUtils editor");

            MonoBehaviour target = _context.target;
            GetObjectViaPath(field.propertyPath, target, out object currentObject);
            
            HandlePropertyField(rect, currentObject.GetType(), target, currentObject, field, label);
        }


        /// <summary>
        /// Get the reference of an object via a path
        /// </summary>
        private static void GetObjectViaPath(string path, MonoBehaviour target, out object result)
        {
            string cachedPath = _context.relativeObjectPath;
            
            if (cachedPath != "" && path.StartsWith(cachedPath)) {
                result = _context.relativeObject;
                path = path[(cachedPath.Length+1)..];
            }
            else {
                result = target;
            }

            Type resultType = result.GetType();
            
            foreach (string s in path.Split('.').SkipLast(1)) {
                result = resultType.GetField(s, fieldBindings)?.GetValue(result);
                if (result == null) throw new Exception($"Property {s} ({path}) does not exist on target");
                
                resultType = result.GetType();
            }
        }
        

        #endregion
        


        #region Styling
        
        private static JUtilsEditorProperties _properties = null;
        public static bool hidden
        {
            get => _properties.hidden;
            set => _properties.hidden = value;
        }

        #endregion
        


        #region Utility
        
        
        /// <summary>
        /// Removes the generic parameters for class names
        /// </summary>
        private static string PurifyTypeName(Type type)
        {
            return PurifyTypeName(type.FullName);
        }


        /// <summary>
        /// Removes the generic parameters for class names
        /// </summary>
        private static string PurifyTypeName(string input)
        {
            string replace = input.Replace("&", "&0");
            string result  = Regex.Replace(replace, "[\\[\\]]", "&1$&");
            IEnumerable<string> split = result.Split("&1").Select(t => t.Replace("&0", "&"));

            int depth = 0;
            string newString = split.First();
            
            foreach (string s in split.Skip(1)) {
                if (s.StartsWith('[')) depth++;
                if (s.StartsWith(']')) depth--;

                if (depth == 0) {
                    newString += s[1..];
                }
            }

            return newString;
        }

        
        #endregion
        

        #endregion
        
        
        #region Loading

        public JUtilsEditor()
        {
            if (_loaded) return;
            
            _generic = new GenericProperty();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<string> excludeTypes = new ();
            List<string> excludeAttributeTypes = new ();
            List<JUtilsAttributeEditor> attributeEditors = new ();
            
            //  Caching types

            Type jUtilsAeType       = typeof(JUtilsAttributeEditor);
            Type propertyDrawerType = typeof(PropertyDrawer);
            Type customPdType       = typeof(CustomPropertyDrawer);
            Type attributeType      = typeof(Attribute);
            
            //  Looping through all types
            
            foreach (Assembly assembly in assemblies) {
                foreach (Type type in assembly.GetTypes()) {
                    
                    //  Checking if it extends JUtilsAttributeEditor

                    if (type.IsSubclassOf(jUtilsAeType)) {
                        attributeEditors.Add(Activator.CreateInstance(type) as JUtilsAttributeEditor);
                    }
                    
                    //  Checking if its a PropertyDrawer
                    
                    else if (type.IsSubclassOf(propertyDrawerType)) {
                        Attribute attribute = type.GetCustomAttribute(customPdType);
                        Type targetType = attribute?.GetType().GetField("m_Type", fieldBindings)?.GetValue(attribute) as Type;
                        if (targetType is null) continue;
                        
                        //  Handling for attribute

                        if (targetType.IsSubclassOf(attributeType)) {
                            excludeAttributeTypes.Add(PurifyTypeName(targetType));
                        }

                        //  Handling for class

                        else {
                            excludeTypes.Add(PurifyTypeName(targetType));
                        }
                    }
                }
                //  Saving

                _loaded = true;
                _excludeTypes = excludeTypes.ToArray();
                _excludeAttributeTypes = excludeAttributeTypes.ToArray();
                _attributeEditors = attributeEditors.ToArray();
            }
        }

        #endregion

        
        #region Drawing
        
        public override void OnInspectorGUI()
        {
            if (_attributeEditors is null) {
                Debug.Log("Failed to initialized _callbackReceivers");
                return;
            }
            if (_excludeTypes is null) {
                Debug.Log("Failed to initialized _excludeTypes");
                return;
            }
            if (_excludeAttributeTypes is null) {
                Debug.Log("Failed to initialized _excludeTypes");
                return;
            }

            _properties = new JUtilsEditorProperties();
            
            MonoBehaviour target = this.target as MonoBehaviour;
            Type type = target.GetType();

            //  Base iterator handling

            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            
            if (type.GetCustomAttributes().Any(t => t is SwappableScript)) {
                EditorGUILayout.PropertyField(iterator);
            }
            else {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(iterator);
                GUI.enabled = true;
            }
            
            //  Going into members

            _context = new JUtilsEditorContext {baseType = type, target = target, relativeObject = target, relativeObjectPath = ""};

            while (iterator.NextVisible(false)) {
                HandlePropertyField(type, target, target, iterator.Copy());
            }

            serializedObject.ApplyModifiedProperties();
            
            //  Handling post callbacks
            
            foreach (JUtilsAttributeEditor receiver in _attributeEditors) {
                receiver.FinishedDrawing(target);
            }

            _context = null;
            _properties = null;
        }


        
        /// <summary>
        /// Handle the property field
        /// </summary>
        private static void HandlePropertyField(Type type, MonoBehaviour target, object relative, SerializedProperty property, GUIContent label = null)
        {
            JUtilsEditorContext oldContext = _context;

            object relativeObject = relative.GetType().GetField(property.name, fieldBindings)!.GetValue(relative);
            if (relativeObject == null) {
                EditorGUILayout.PropertyField(property, label);
                return;
            }
            
            _context = new JUtilsEditorContext()
            {
                baseType = target.GetType(),
                target = target,
                relativeObject = relative.GetType().GetField(property.name, fieldBindings)!.GetValue(relative),
                relativeObjectPath = property.propertyPath
            };
            
            List<ReceiverContext> receivers = new ();
            
            //  Getting field
            
            FieldInfo fieldInfo = type.GetField(property.name, fieldBindings);
            if (fieldInfo is null) {
                Debug.LogError($"Could not get FieldInfo on {property.propertyPath}");
                return;
            }
            
            //  Parsing info

            JUtilsEditorInfo info = new ()
            {
                property = property,
                type = type,
                owner = target,
                parentObject = relative,
                currentObject = fieldInfo.GetValue(relative),
                field = fieldInfo,
                label = label ?? new GUIContent
                {
                    text = property.displayName,
                    tooltip = property.tooltip
                }
            };
            
            //  Getting custom attributes
            
            foreach (JUtilsAttributeEditor receiver in _attributeEditors) {
                if (!Attribute.IsDefined(fieldInfo, receiver.targetAttribute)) continue;
                Attribute attribute = fieldInfo.GetCustomAttribute(receiver.targetAttribute, true);
                
                //  Adding to list

                info.attribute = attribute;

                try {
                    receiver.PreFieldDrawn(info);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }
                finally {
                    receivers.Add(new ReceiverContext {attribute = attribute, receiver = receiver});
                }
            }
            
            //  Handling

            if (!hidden) {
                bool overridden = false;
                
                foreach (ReceiverContext receiver in receivers) {
                    info.attribute = receiver.attribute;

                    try {
                        if (!receiver.receiver.OverrideFieldDraw(info, info.label)) continue;
                    }
                    catch (Exception e) {
                        Debug.LogException(e);
                    }
                    
                    overridden = true;
                    break;
                }
                
                //  Drawing sub classes

                if (!overridden) {
                    if(property.propertyType != SerializedPropertyType.Generic)
                        EditorGUILayout.PropertyField(property, info.label);
                    else {
                        Type newType = info.currentObject.GetType();
                        
                        SerializedProperty copy = property.Copy();
                        string name = PurifyTypeName(newType);
                        
                        if (_excludeTypes.Any(t => t == name))
                            EditorGUILayout.PropertyField(copy, info.label);
                        else if (info.field.CustomAttributes.Any(t => { string purified = PurifyTypeName(t.AttributeType); return _excludeAttributeTypes.Any(t => t == purified);}))
                            EditorGUILayout.PropertyField(copy, info.label);
                        else _generic.OnGUI(copy, info.label);
                    }
                }
            }

            //  Post drawn
            
            foreach (ReceiverContext receiver in receivers) {
                try {
                    info.attribute = receiver.attribute;
                    receiver.receiver.PostFieldDrawn(info);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }
            }

            _context = oldContext;
        }
        
        
        /// <summary>
        /// Handle the property with a fixed rect
        /// </summary>
        private static void HandlePropertyField(Rect rect, Type type, MonoBehaviour target, object relative, SerializedProperty property, GUIContent label = null)
        {
            List<ReceiverContext> receivers = new ();
            
            //  Getting field
            
            FieldInfo fieldInfo = type.GetField(property.name, fieldBindings);
            if (fieldInfo is null) {
                Debug.LogError($"Could not get FieldInfo on {property.propertyPath}");
                return;
            }
            
            //  Parsing info

            JUtilsEditorInfo info = new ()
            {
                property = property,
                type = type,
                owner = target,
                parentObject = relative,
                currentObject = fieldInfo.GetValue(relative),
                field = fieldInfo,
                label = label ?? new GUIContent
                {
                    text = property.displayName,
                    tooltip = property.tooltip
                }
            };
            
            //  Getting custom attributes
            
            foreach (JUtilsAttributeEditor receiver in _attributeEditors) {
                if (!Attribute.IsDefined(fieldInfo, receiver.targetAttribute)) continue;
                Attribute attribute = fieldInfo.GetCustomAttribute(receiver.targetAttribute, true);
                
                //  Adding to list

                info.attribute = attribute;

                try {
                    receiver.PreFieldDrawn(info);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }
                finally {
                    receivers.Add(new ReceiverContext {attribute = attribute, receiver = receiver});
                }
            }
            
            //  Handling

            if (!hidden) {
                bool overridden = false;
                
                foreach (ReceiverContext receiver in receivers) {
                    info.attribute = receiver.attribute;

                    try {
                        if (!receiver.receiver.OverrideFieldDraw(info, info.label)) continue;
                    }
                    catch (Exception e) {
                        Debug.LogException(e);
                    }
                    
                    overridden = true;
                    break;
                }
                
                //  Drawing sub classes

                if (!overridden) {
                    if(property.propertyType != SerializedPropertyType.Generic)
                        EditorGUI.PropertyField(rect, property, info.label);
                    else {
                        Type newType = info.currentObject.GetType();

                        SerializedProperty copy = property.Copy();
                        string name = PurifyTypeName(newType);
                        
                        if (_excludeTypes.Any(t => t == name))
                            EditorGUI.PropertyField(rect, property, info.label);
                        else _generic.OnGUI(rect, copy, info.label);
                    }
                }
            }

            //  Post drawn
            
            foreach (ReceiverContext receiver in receivers) {
                try {
                    info.attribute = receiver.attribute;
                    receiver.receiver.PostFieldDrawn(info);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }
        
        
        
        private struct ReceiverContext
        {
            public JUtilsAttributeEditor receiver;
            public Attribute attribute;
        }
        
        
        private class PurifiedTypePair
        {
            public Type type;
            public string name;
        }
        
        
        
        private class JUtilsEditorContext
        {
            public Type baseType;
            public MonoBehaviour target;
            public string relativeObjectPath;
            public object relativeObject;
        }

        #endregion
    }



    public class JUtilsEditorProperties
    {
        public bool hidden;
    }



    public struct JUtilsEditorInfo
    {
        public Type type;
        public SerializedProperty property;
        public MonoBehaviour owner;
        
        public FieldInfo field;
        public object parentObject;
        public object currentObject;
        public GUIContent label;
        public Attribute attribute;
    }
    

    public abstract class JUtilsAttributeEditor
    {
        public abstract Type targetAttribute { get; }


        public virtual void PreFieldDrawn(JUtilsEditorInfo info) {}
        public virtual bool OverrideFieldDraw(JUtilsEditorInfo info, GUIContent label) => false;
        public virtual void PostFieldDrawn(JUtilsEditorInfo info) {}
        public virtual void FinishedDrawing(MonoBehaviour target) {}
    }
#endif
}
