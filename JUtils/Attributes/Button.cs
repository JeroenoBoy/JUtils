using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using JUtils.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;



namespace JUtils.Attributes
{
    /// <summary>
    /// Attach this button to a method to make it clickable in the inspector. This attribute will also draw parameters if it can.
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class ButtonExample : MonoBehaviour
    ///     {
    ///         [Button]
    ///         public void SimpleButton() {
    ///             Debug.Log("The Simple button has been pressed");
    ///         }
    ///     }  
    /// }
    /// </code></example>
    [AttributeUsage(AttributeTargets.Method)]
    public class Button : Attribute
    {
        [CanBeNull] public readonly string Name;
        public bool ClickableInEditor;
        
        
        /// <summary>
        /// Create an inspector button for this atribute
        /// </summary>
        /// <param name="name">The name of the button</param>
        /// <param name="clickableInEditor">Should the button only run in playmode</param>
        public Button(
            [CanBeNull] string name = null,
            bool clickableInEditor = false)
        {
            Name = name;
            ClickableInEditor = clickableInEditor;
        }
    }


#if UNITY_EDITOR
        
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class ButtonEditor : UnityEditor.Editor
    {
        private InspectorButton[] _buttons;
        
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

            _buttons ??= new InspectorButton[methods.Length];
            if (_buttons.Length != methods.Length) Array.Resize(ref _buttons, methods.Length);
            
            for (int i = 0; i < methods.Length; i++) {
                _buttons[i] ??= new InspectorButton();
                _buttons[i].Draw(behaviour, methods[i] as MethodInfo);
            }
        }
        
        

        public class InspectorButton
        {
            private bool _isOpen;
            private bool _inCoroutine;
            private object[] _params;


            /// <summary>
            /// Draws the button itself
            /// </summary>
            public void Draw(MonoBehaviour behaviour, MethodInfo method)
            {
                Button attribute  = method.GetCustomAttributes(typeof(Button), true).First() as Button;
                string name       = attribute.Name ?? PrettifyName(method.Name);
                
                //  Checking if the button is a coroutine
                
                bool isCoroutine = method.ReturnType == typeof(IEnumerator);

                //  Checking if the button is playmode only
                
                if ((!attribute.ClickableInEditor || isCoroutine) && !Application.isPlaying)
                {
                    GUI.enabled = false;
                    bool pressed = DrawButtonInGui(method, name);
                    GUI.enabled = true;

                    if (pressed)
                        Debug.LogWarning("Button is only available in play mode");
                    
                    return;
                }
                
                //  Checking if the button was clicked
                
                if (!DrawButtonInGui(method, name)) return;
                
                //  Invoking method

                if (isCoroutine) {
                    behaviour.StartCoroutine(CoroutineWrapper(method.Invoke(behaviour, _params) as IEnumerator));
                }
                else {
                    method.Invoke(behaviour, _params);
                }
            }
            
            
            /// <summary>
            /// Draws the button and its methods in the gui
            /// </summary>
            private bool DrawButtonInGui(MethodInfo info, string name)
            {
                ParameterInfo[] parameters = info.GetParameters();
    
                //  Without parameters
                
                if (parameters.Length == 0)
                    return GUILayout.Button(name);
                
                //  With parameters
    
                EditorGUILayout.BeginVertical(GUI.skin.box);
                
                bool pressed = GUILayout.Button(name);
        
                //  Handling parameters
    
                _params ??= new object[parameters.Length];
                
                if (_params.Length != parameters.Length)
                    Array.Resize(ref _params, parameters.Length);
    
                for (int i = 0; i < parameters.Length; i++) {
                    DrawParameter(i, parameters[i]);
                }
                
                //  Returning
                
                EditorGUILayout.EndVertical();
                return pressed;
            }
            
    
            /// <summary>
            /// Draws and updates a parameter for the button
            /// </summary>
            private void DrawParameter(int i, ParameterInfo info)
            {
                Type type = info.ParameterType;
                string name = PrettifyName(info.Name);
    
                if (type == typeof(bool)) {
                    if (_params[i] is not bool) _params[i] = info.HasDefaultValue ? info.DefaultValue : false;
                    _params[i] = EditorGUILayout.Toggle(name, (bool)_params[i]);
                }
                else if (type == typeof(string)) {
                    if (_params[i] is not string) _params[i] = info.HasDefaultValue ? info.DefaultValue : "";
                    _params[i] = EditorGUILayout.TextField(name, (string)_params[i]);
                }
                
                //  Numbers
                
                else if (type == typeof(int)) {
                    if (_params[i] is not int) _params[i] = info.HasDefaultValue ? info.DefaultValue : 0;
                    _params[i] = EditorGUILayout.IntField(name, (int)_params[i]);
                }
                else if (type == typeof(float)) {
                    if (_params[i] is not float) _params[i] = info.HasDefaultValue ? info.DefaultValue : 0f;
                    _params[i] = EditorGUILayout.FloatField(name, (float)_params[i]);
                }
                else if (type == typeof(double)) {
                    if (_params[i] is not double) _params[i] = info.HasDefaultValue ? info.DefaultValue : 0d;
                    _params[i] = EditorGUILayout.DoubleField(name, (double)_params[i]);
                }
                else if (type == typeof(long)) {
                    if (_params[i] is not long) _params[i] = info.HasDefaultValue ? info.DefaultValue : 0;
                    _params[i] = EditorGUILayout.LongField(name, (long)_params[i]);
                }
                
                //  Vectors
                
                else if (type == typeof(Vector2)) {
                    if (_params[i] is not Vector2) _params[i] = info.HasDefaultValue ? info.DefaultValue : Vector2.zero;
                    _params[i] = EditorGUILayout.Vector2Field(name, (Vector2)_params[i]);
                }
                
                else if (type == typeof(Vector3)) {
                    if (_params[i] is not Vector3) _params[i] = info.HasDefaultValue ? info.DefaultValue : Vector3.zero;
                    _params[i] = EditorGUILayout.Vector3Field(name, (Vector3)_params[i]);
                }
                else if (type == typeof(Vector4)) {
                    if (_params[i] is not Vector4) _params[i] = info.HasDefaultValue ? info.DefaultValue : Vector4.zero;
                    _params[i] = EditorGUILayout.Vector4Field(name, (Vector4)_params[i]);
                }
                else if (type == typeof(Vector2Int)) {
                    if (_params[i] is not Vector2Int) _params[i] = info.HasDefaultValue ? info.DefaultValue : Vector2Int.zero;
                    _params[i] = EditorGUILayout.Vector2IntField(name, (Vector2Int)_params[i]);
                }
                else if (type == typeof(Vector3Int)) {
                    if (_params[i] is not Vector3Int) _params[i] = info.HasDefaultValue ? info.DefaultValue : Vector3Int.zero;
                    _params[i] = EditorGUILayout.Vector3IntField(name, (Vector3Int)_params[i]);
                }
                
                //  Other

                else if (type == typeof(Rect)) {
                    if (_params[i] is not Rect) _params[i] = info.HasDefaultValue ? info.DefaultValue : Rect.zero;
                    _params[i] = EditorGUILayout.RectField(name, (Rect)_params[i]);
                }
                else if (type == typeof(RectInt)) {
                    if (_params[i] is not RectInt) _params[i] = info.HasDefaultValue ? info.DefaultValue : default(Bounds);
                    _params[i] = EditorGUILayout.RectIntField(name, (RectInt)_params[i]);
                }
                else if (type == typeof(Bounds)) {
                    if (_params[i] is not Bounds) _params[i] = info.HasDefaultValue ? info.DefaultValue : default(Bounds);
                    _params[i] = EditorGUILayout.BoundsField(name, (Bounds)_params[i]);
                }
                
                else if (type == typeof(Color)) {
                    if (_params[i] is not Color) _params[i] = info.HasDefaultValue ? info.DefaultValue : Color.black;
                    _params[i] = EditorGUILayout.ColorField(name, (Color)_params[i]);
                }
                
                else if (type == typeof(LayerMask)) {
                    if (_params[i] is not LayerMask) _params[i] = info.HasDefaultValue ? info.DefaultValue : default(LayerMask);
                    _params[i] = (LayerMask)EditorGUILayout.LayerField(name, (LayerMask)_params[i]);
                }
                
                
                
                else {

                    //  Getting Object type
                    
                    Type baseType = type;
                    while (baseType != typeof(object)) {
                        if (baseType == typeof(Object)) {
                            if (_params[i] is not Object) _params[i] = info.HasDefaultValue ? info.DefaultValue : null;

                            _params[i] = EditorGUILayout.ObjectField(name, (Object)_params[i], type, true);
                            return;
                        }
                        baseType = baseType.BaseType;
                    }
                    
                    //  Invalid
                    
                    Debug.LogError($"Cannot process argument type {type}");
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
                
                CoroutineCatcher catcher = Routines.Catcher(coroutine);
                yield return catcher;
    
                //  Ending
                
                _inCoroutine = false;
                
                if (catcher.HasThrown(out Exception exception)) {
                    Debug.LogError(exception);
                }
            }
        }


        
        
        private static readonly Regex _regex = new ("([A-Z])");
        private static string PrettifyName(string name)
        {
            return char.ToUpper(name[0]) + _regex.Replace(name[1..], " $&").Trim();
        }
    }
    
#endif
}
