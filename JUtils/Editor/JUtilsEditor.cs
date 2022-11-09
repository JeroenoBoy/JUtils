﻿
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
        private JUtilsAttributeEditor[] _callbackReceivers;
        
        public JUtilsEditor()
        {
            Type postCallbackType = typeof(JUtilsAttributeEditor);
            
            _callbackReceivers = Assembly.GetAssembly(postCallbackType).GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(postCallbackType))
                .Select(t => Activator.CreateInstance(t) as JUtilsAttributeEditor)
                .ToArray();
        }


        public override void OnInspectorGUI()
        {
            if (_callbackReceivers is null) return;
            
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
        }


        private void LoopRecursive(Type type, MonoBehaviour target, SerializedProperty property)
        {
            int depth = property.depth;
            List<ReceiverContext> receivers = new ();

            //  Looping through all the children
            
            do {
                FieldInfo fieldInfo = type.GetField(property.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

                JUtilsEditorInfo info = new ()
                {
                    property = property,
                    type = type,
                    target = target,
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

                bool overridden = false;
                
                foreach (ReceiverContext receiver in receivers) {
                    info.attribute = receiver.attribute;
                    if (!receiver.receiver.OverrideFieldDraw(info)) continue;
                    overridden = true;
                    break;
                }
                
                if (!overridden) EditorGUILayout.PropertyField(property);
                
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
    }



    public struct JUtilsEditorInfo
    {
        public Type type;
        public FieldInfo info;
        public SerializedProperty property;
        public MonoBehaviour target;
        
        public Attribute attribute;
    }
    

    public abstract class JUtilsAttributeEditor
    {
        public abstract Type targetAttribute { get; }


        public virtual void PreFieldDrawn(JUtilsEditorInfo info) {}
        public virtual bool OverrideFieldDraw(JUtilsEditorInfo info) => false;
        public virtual void PostFieldDrawn(JUtilsEditorInfo info) {}
        public virtual void PostCallback(MonoBehaviour target) {}
    }
#endif
}
