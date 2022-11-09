
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;



namespace JUtils.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class JUtilsEditor : UnityEditor.Editor
    {
        private JUtilsEditorCallbackReceiver[] _callbackReceivers;
        
        public JUtilsEditor()
        {
            Type postCallbackType = typeof(JUtilsEditorCallbackReceiver);
            
            _callbackReceivers = Assembly.GetAssembly(postCallbackType).GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(postCallbackType))
                .Select(t => Activator.CreateInstance(t) as JUtilsEditorCallbackReceiver)
                .ToArray();
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
         
            if (_callbackReceivers is null) return;
            
            MonoBehaviour target = this.target as MonoBehaviour;
            Type type = target.GetType();
            
            LoopRecursive(type, target, serializedObject.GetIterator());
            
            foreach (JUtilsEditorCallbackReceiver receiver in _callbackReceivers) {
                receiver.PostCallback(target);
            }
        }


        private void LoopRecursive(Type type, MonoBehaviour target, SerializedProperty property)
        {
            int depth = property.depth;
            List<JUtilsEditorCallbackReceiver> receivers = new ();

            //  Looping through all the children
            
            do {
                foreach (CustomAttributeData attribute in type.GetProperty(property.name)!.CustomAttributes) {
                    Type attributeType = attribute.AttributeType;

                    //  Getting custom attributes
                    
                    foreach (JUtilsEditorCallbackReceiver reciever in _callbackReceivers) {
                        if (attributeType != reciever.targetAttribute) continue;
                        receivers.Add(reciever);
                    }
                    
                    //  Handling 
                    
                    receivers.Clear();
                }
                
            } while (property.NextVisible(false));
        }
    }



    public abstract class JUtilsEditorCallbackReceiver
    {
        public abstract Type targetAttribute { get; }

        
        public virtual void PreFieldDrawn(Type type, Attribute attribute, MonoBehaviour target) {}
        public virtual void PostCallback(MonoBehaviour target) {}
    }
#endif
}
