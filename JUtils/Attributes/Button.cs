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
        
        
        /// <summary>
        /// Create an inspector button for this atribute
        /// </summary>
        /// <param name="name">The name of the button</param>
        /// <param name="playModeOnly">Should the button only run in playmode</param>
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
            MonoBehaviour behaviour = target as MonoBehaviour;
 
            //  Get all methods with Button attribute
            
            MemberInfo[] methods = behaviour.GetType()
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(o => Attribute.IsDefined(o, typeof (Button)))
                .ToArray();

            //  Draw buttons
            
            if (methods.Length == 0) return;
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            //  Draw buttons
            
            foreach (MemberInfo info in methods) {
                Button attribute = info.GetCustomAttributes(typeof(Button), true).First() as Button;
                string name      = attribute.name ?? PrettifyName(info.Name);

                //  Checking if the button is playmode only
                
                if (attribute.playModeOnly && !Application.isPlaying)
                {
                    GUI.enabled = false;
                    bool pressed = GUILayout.Button(name);
                    GUI.enabled = true;

                    if (pressed)
                        Debug.LogWarning("Button is only available in play mode");
                    
                    continue;
                }
                
                //  Checking if the button was clicked
                
                if (!GUILayout.Button(name)) continue;
                
                //  Double checking playmode
                
                if (attribute.playModeOnly && !Application.isPlaying) {
                    Debug.LogWarning("Button is only available in play mode");
                    continue;
                }
                
                //  Invoking method
                    
                MethodInfo method = info as MethodInfo;
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
