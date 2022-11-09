using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Codice.CM.Common.Serialization;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class Required : PropertyAttribute
    {
#if UNITY_EDITOR

        [CustomPropertyDrawer(typeof(Required))]
        private class RequiredEditor : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUI.GetPropertyHeight(property, true);
            }
        
        
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.PropertyField(position, property, label, true);

                Debug.Log(property.propertyPath);
                object obj = property.serializedObject.targetObject;

                if (property.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox($"Field \"{property.displayName}\" is required", MessageType.Error);
                    EditorGUILayout.Space(4);
                }
            }
        }
#endif
    }
    
    
    
    public class ExperimentalRequired : PropertyAttribute
    {
        private bool _isUnityObject;
        [CanBeNull]
        private string _relativePath;


        public ExperimentalRequired() { }
        public ExperimentalRequired(bool isUnityObject = true, [CanBeNull] string relativePath = null)
        {
            _isUnityObject = isUnityObject;
            _relativePath = relativePath;
        }


#if UNITY_EDITOR

        [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
        public class RequiredEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                IEnumerable<ITest> classes = Assembly.GetAssembly(typeof(ITest))
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ITest)))
                    .Select(t => (ITest)Activator.CreateInstance(t, new { }));
                
                
                
                Type type = serializedObject.targetObject.GetType(); 
                Debug.Log("Hi1");
                foreach (SerializedProperty property in serializedObject.GetIterator()) {
                    Debug.Log(property.propertyPath);
                }
                
                base.OnInspectorGUI();
            }
        }
        
        
        public interface ITest
        {
            
        }



        public interface IJUtilsEditor { }
#endif
    }
}
