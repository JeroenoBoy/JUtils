using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

using UnityEditor;

using UnityEngine;



namespace JUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Button : Attribute
    {
        [CanBeNull] public string name;
        public bool playModeOnly;
        
        
        public Button(
            [CanBeNull] string name = null,
            bool playModeOnly = true)
        {
            this.name = name;
            this.playModeOnly = playModeOnly;
        }
    }


#if UNITY_EDITOR
        
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class ButtonEditor : UnityEditor.Editor
    {
        // private bool _enabled;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var behaviour = target as MonoBehaviour;
 
            //  Get all methods with Button attribute
            
            var methods = behaviour.GetType()
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(o => Attribute.IsDefined(o, typeof (Button)))
                .ToArray();

            //  Draw buttons
            
            if (methods.Length == 0) return;
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            //  Draw buttons
            
            foreach (var info in methods) {
                var attribute = info.GetCustomAttributes(typeof(Button), true)
                    .First() as Button;
                
                var name = attribute.name ?? PrettifyName(info.Name);

                if (attribute.playModeOnly && !Application.isPlaying)
                {
                    GUI.enabled = false;
                    bool pressed = GUILayout.Button(name);
                    GUI.enabled = true;

                    if (pressed)
                        Debug.LogWarning("Button is only available in play mode");
                    
                    continue;
                }
                
                if (!GUILayout.Button(name)) continue;
                
                if (attribute.playModeOnly && !Application.isPlaying) {
                    Debug.LogWarning("Button is only available in play mode");
                    continue;
                }
                    
                var method = info as MethodInfo;
                method.Invoke(behaviour, null);
            }
        }
        
        
        private static readonly Regex _regex = new ("([A-Z])");
        private static string PrettifyName(string name)
        {
            return _regex.Replace(name, " $&").Trim();
        } 
    }
    
#endif
}
