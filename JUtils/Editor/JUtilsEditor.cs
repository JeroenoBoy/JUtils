
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JUtils.Attributes;
using UnityEditor;
using UnityEngine;



namespace JUtils.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public sealed class JUtilsEditor : UnityEditor.Editor
    {
        #region Static properties

        private static JUtilsEditorProperties _properties = null;


        public static bool hidden
        {
            get => _properties.hidden;
            set => _properties.hidden = value;
        }
        

        #endregion
        
        
        #region Loading
        
        private JUtilsAttributeEditor[] _callbackReceivers;
        
        public JUtilsEditor()
        {
            Type postCallbackType = typeof(JUtilsAttributeEditor);
            
            _callbackReceivers = Assembly.GetAssembly(postCallbackType).GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(postCallbackType))
                .Select(t => Activator.CreateInstance(t) as JUtilsAttributeEditor)
                .ToArray();
        }

        #endregion


        #region Drawing
        
        public override void OnInspectorGUI()
        {
            if (_callbackReceivers is null) {
                Debug.Log("Failed to initialized _callbackReceivers");
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
            
            if (iterator.NextVisible(true)) LoopRecursive(type, target, iterator);

            serializedObject.ApplyModifiedProperties();
            
            //  Handling post callbacks
            
            foreach (JUtilsAttributeEditor receiver in _callbackReceivers) {
                receiver.PostCallback(target);
            }

            _properties = null;
        }


        private void LoopRecursive(Type type, MonoBehaviour target, SerializedProperty property)
        {
            List<ReceiverContext> receivers = new ();

            //  Looping through all the children
            
            do {
                FieldInfo fieldInfo = type.GetField(property.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

                JUtilsEditorInfo info = new ()
                {
                    property = property,
                    type = type,
                    target = target,
                    label = new GUIContent()
                    {
                        text = property.displayName,
                        tooltip = property.tooltip
                    }
                };
                
                //  Getting custom attributes
                
                foreach (JUtilsAttributeEditor receiver in _callbackReceivers) {
                    if (!Attribute.IsDefined(fieldInfo, receiver.targetAttribute)) continue;
                    Attribute attribute = fieldInfo.GetCustomAttribute(receiver.targetAttribute, true);
                    
                    //  Adding to list

                    info.attribute = attribute;
                    
                    receiver.PreFieldDrawn(info);
                    receivers.Add(new ReceiverContext { attribute = attribute, receiver = receiver});
                }
                
                //  Handling

                if (!hidden) {
                    bool overridden = false;
                    
                    foreach (ReceiverContext receiver in receivers) {
                        info.attribute = receiver.attribute;
                        if (!receiver.receiver.OverrideFieldDraw(info, info.label)) continue;
                        overridden = true;
                        break;
                    }
                    
                    if (!overridden) EditorGUILayout.PropertyField(property, info.label);
                }

                //  Post drawn
                
                foreach (ReceiverContext receiver in receivers) {
                    info.attribute = receiver.attribute;
                    receiver.receiver.PostFieldDrawn(info);
                }

                receivers.Clear();
            } while (property.NextVisible(false));
        }
        
        
        private struct ReceiverContext
        {
            public JUtilsAttributeEditor receiver;
            public Attribute attribute;
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
        public FieldInfo info;
        public SerializedProperty property;
        public MonoBehaviour target;
        public GUIContent label;
        
        public Attribute attribute;
    }
    

    public abstract class JUtilsAttributeEditor
    {
        public abstract Type targetAttribute { get; }


        public virtual void PreFieldDrawn(JUtilsEditorInfo info) {}
        public virtual bool OverrideFieldDraw(JUtilsEditorInfo info, GUIContent label) => false;
        public virtual void PostFieldDrawn(JUtilsEditorInfo info) {}
        public virtual void PostCallback(MonoBehaviour target) {}
    }
#endif
}
