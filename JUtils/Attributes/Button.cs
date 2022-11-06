using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using JUtils.Extensions;
using UnityEditor;

using UnityEngine;



namespace JUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Button : Attribute
    {
        [CanBeNull] public string name;
        public bool clickableInEditor;
        
        
        /// <summary>
        /// Create an inspector button for this atribute
        /// </summary>
        /// <param name="name">The name of the button</param>
        /// <param name="clickableInEditor">Should the button only run in playmode</param>
        /// <param name="playModeOnly">Deprecated Param, only here for compatability sake</param>
        /// 
        public Button(
            [CanBeNull] string name = null,
            bool clickableInEditor = false,
            bool playModeOnly = true )
        {
            this.name = name;
            this.clickableInEditor = clickableInEditor || !playModeOnly;
        }
    }


#if UNITY_EDITOR
        
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class ButtonEditor : UnityEditor.Editor
    {
        private bool _inCoroutine;
        
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
                Button attribute  = info.GetCustomAttributes(typeof(Button), true).First() as Button;
                string name       = attribute.name ?? PrettifyName(info.Name);
                MethodInfo method = info as MethodInfo;
                
                //  Checking if the method has arguments

                System.Diagnostics.Debug.Assert(method != null, nameof(method) + " != null");
                if (method.GetParameters().Length > 0) throw new Exception("No parameters allowed for buttons");
                
                //  Checking if the button is a coroutine
                
                bool isCoroutine = method.ReturnType == typeof(IEnumerator);

                //  Checking if the button is playmode only
                
                if ((attribute.clickableInEditor || isCoroutine) && !Application.isPlaying)
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
                
                if (attribute.clickableInEditor && !Application.isPlaying) {
                    Debug.LogWarning("Button is only available in play mode");
                    continue;
                }
                
                //  Invoking method

                if (isCoroutine) {
                    behaviour.StartCoroutine(CoroutineWrapper(method.Invoke(behaviour, null) as IEnumerator));
                }
                else {
                    method.Invoke(behaviour, null);
                }
            }
        }


        /// <summary>
        /// This is a wrapper for the coroutine and checks it for errors
        /// </summary>
        private IEnumerator CoroutineWrapper(IEnumerator coroutine)
        {
            //  Checking if its already running
            
            if (_inCoroutine) {
                Debug.LogWarning("Coroutine is currently running!");
                yield break;
            }

            _inCoroutine = true;
            
            //  Running coroutine
            
            CoroutineCatcher catcher = Coroutines.Catcher(coroutine);
            yield return catcher;

            //  Ending
            
            _inCoroutine = false;
            
            if (catcher.HasThrown(out Exception exception)) {
                Debug.LogError(exception);
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
