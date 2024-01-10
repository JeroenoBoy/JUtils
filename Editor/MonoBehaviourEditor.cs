using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace JUtils.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class MonoBehaviourEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            MonoBehaviour behaviour = (MonoBehaviour)target;
            MemberInfo[] methods = behaviour.GetType()
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(o => Attribute.IsDefined(o, typeof (ButtonAttribute)))
                .ToArray();
            
            if (methods.Length == 0) return root;
            
            foreach (MemberInfo memberInfo in methods) {
                root.Add(new JUtilsInspectorButton(behaviour, memberInfo as MethodInfo));
            }
            
            return root;
        }
    }
}