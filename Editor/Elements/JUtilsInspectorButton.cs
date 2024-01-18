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
    public class JUtilsInspectorButton : VisualElement
    {
        private bool isCoroutineButton => _methodInfo.ReturnType == typeof(IEnumerator);
        private bool canClickButton => Application.isPlaying || (_attribute.clickableInEditor && !isCoroutineButton);
        
        private bool _isOpen;
        private bool _inCoroutine;
        private object[] _params;

        private readonly MonoBehaviour _behaviour;
        private readonly MethodInfo _methodInfo;
        private readonly ButtonAttribute _attribute;

        private VisualElement _buttonElement;


        public JUtilsInspectorButton(MonoBehaviour behaviour, MethodInfo methodInfo)
        {
            _behaviour = behaviour;
            _methodInfo = methodInfo;
            _attribute = (ButtonAttribute)_methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true).First();
            string label = _attribute.name ?? PrettifyName(methodInfo.Name);

            EditorApplication.playModeStateChanged += HandlePlayModeChanged;
            RegisterCallback((DetachFromPanelEvent _) => EditorApplication.playModeStateChanged -= HandlePlayModeChanged);

            Add(CreateButtonElement(label));
            HandlePlayModeChanged((PlayModeStateChange)(-1));
        }


        /// <summary>
        /// Draws the button and its methods in the gui
        /// </summary>
        private VisualElement CreateButtonElement(string label)
        {
            Button button = new();
            button.Add(new Label(label));
            button.clicked += HandleButtonClicked;

            _buttonElement = button;
            
            ParameterInfo[] parameters = _methodInfo.GetParameters();
            if (parameters.Length == 0) return button;

            VisualElement root = new();
            root.Add(button);
            
            root.style.backgroundColor = new Color(.19f, .19f, .19f);
            root.style.paddingTop = 2;
            root.style.paddingBottom = 2;
            root.style.paddingLeft = 2;
            root.style.paddingRight = 2;
            root.style.borderTopLeftRadius = 4;
            root.style.borderTopRightRadius = 4;
            root.style.borderBottomLeftRadius = 4;
            root.style.borderBottomRightRadius = 4;
            root.style.marginTop = 2;
            
            _params ??= new object[parameters.Length];
            
            if (_params.Length != parameters.Length) Array.Resize(ref _params, parameters.Length);

            for (int i = 0; i < parameters.Length; i++) {
                root.Add(DrawParameter(i, parameters[i]));
            }
            
            return root;
        }
        

        /// <summary>
        /// Draws and updates a parameter for the button
        /// </summary>
        private VisualElement DrawParameter(int index, ParameterInfo info)
        {
            Type type = info.ParameterType;
            string label = PrettifyName(info.Name);

            if (type == typeof(bool)) {
                if (_params[index] is not bool) _params[index] = info.HasDefaultValue ? info.DefaultValue : false;
                return RegisterField<bool>(index, new Toggle(label));
            }
            if (type == typeof(string)) {
                if (_params[index] is not string) _params[index] = info.HasDefaultValue ? info.DefaultValue : "";
                return RegisterField<string>(index, new TextField(label));
            }
            
            //  Numbers

            if (type == typeof(int)) {
                if (_params[index] is not int) _params[index] = info.HasDefaultValue ? info.DefaultValue : 0;
                return RegisterField<int>(index, new IntegerField(label));
            }
            if (type == typeof(float)) {
                if (_params[index] is not float) _params[index] = info.HasDefaultValue ? info.DefaultValue : 0f;
                return RegisterField<float>(index, new FloatField(label));
            }
            if (type == typeof(double)) {
                if (_params[index] is not double) _params[index] = info.HasDefaultValue ? info.DefaultValue : 0d;
                return RegisterField<double>(index, new DoubleField(label));
            }
            if (type == typeof(long)) {
                if (_params[index] is not long) _params[index] = info.HasDefaultValue ? info.DefaultValue : 0L;
                return RegisterField<long>(index, new LongField(label));
            }
            
            //  Vectors
            
            if (type == typeof(Vector2)) {
                if (_params[index] is not Vector2) _params[index] = info.HasDefaultValue ? info.DefaultValue : Vector2.zero;
                return RegisterField<Vector2>(index, new Vector2Field(label));
            }
            if (type == typeof(Vector3)) {
                if (_params[index] is not Vector3) _params[index] = info.HasDefaultValue ? info.DefaultValue : Vector3.zero;
                return RegisterField<Vector2>(index, new Vector3Field(label));
            }
            if (type == typeof(Vector4)) {
                if (_params[index] is not Vector4) _params[index] = info.HasDefaultValue ? info.DefaultValue : Vector4.zero;
                return RegisterField<Vector4>(index, new Vector4Field(label));
            }
            if (type == typeof(Vector2Int)) {
                if (_params[index] is not Vector2Int) _params[index] = info.HasDefaultValue ? info.DefaultValue : Vector2Int.zero;
                return RegisterField<Vector2Int>(index, new Vector2IntField(label));
            }
            if (type == typeof(Vector3Int)) {
                if (_params[index] is not Vector3Int) _params[index] = info.HasDefaultValue ? info.DefaultValue : Vector3Int.zero;
                return RegisterField<Vector3Int>(index, new Vector3IntField(label));
            }
            
            //  Other

            if (type == typeof(Rect)) {
                if (_params[index] is not Rect) _params[index] = info.HasDefaultValue ? info.DefaultValue : Rect.zero;
                return RegisterField<Rect>(index, new RectField(label));
            }
            if (type == typeof(RectInt)) {
                if (_params[index] is not RectInt) _params[index] = info.HasDefaultValue ? info.DefaultValue : default(Bounds);
                return RegisterField<RectInt>(index, new RectIntField(label));
            }
            if (type == typeof(Bounds)) {
                if (_params[index] is not Bounds) _params[index] = info.HasDefaultValue ? info.DefaultValue : default(Bounds);
                return RegisterField<Bounds>(index, new BoundsField(label));
            }
            if (type == typeof(Color)) {
                if (_params[index] is not Color) _params[index] = info.HasDefaultValue ? info.DefaultValue : Color.black;
                return RegisterField<Color>(index, new ColorField(label));
            }
            if (type == typeof(LayerMask)) {
                if (_params[index] is not LayerMask) _params[index] = info.HasDefaultValue ? info.DefaultValue : default(LayerMask);
                return RegisterField<LayerMask>(index, new LayerMaskField(label));
            }

            Type baseType = type;
            while (baseType != typeof(object)) {
                if (baseType == typeof(Object)) {
                    if (_params[index] is not Object) _params[index] = info.HasDefaultValue ? info.DefaultValue : null;
                    return RegisterField<Object>(index, new ObjectField(label) { objectType = type });
                }
                baseType = baseType.BaseType;
            }
            
            Debug.LogError($"Cannot process argument type {type}");
            return null;
        }


        private VisualElement RegisterField<T>(int index, VisualElement field)
        {
            field.RegisterCallback((ChangeEvent<T> e) => _params[index] = e.newValue);
            return field;
        }
        
        
        /// <summary>
        /// Draws the button itself
        /// </summary>
        private void HandleButtonClicked()
        {
            bool isCoroutine = _methodInfo.ReturnType == typeof(IEnumerator);

            if (!canClickButton) {
                Debug.LogWarning("This button is currently not clickable");
                return;
            }

            if (isCoroutine) {
                _behaviour.StartCoroutine(CoroutineWrapper(_methodInfo.Invoke(_behaviour, _params) as IEnumerator));
            } else {
                _methodInfo.Invoke(_behaviour, _params);
            }
        }

        
        private void HandlePlayModeChanged(PlayModeStateChange stateChange)
        {
            _buttonElement.SetEnabled(canClickButton);
        }


        /// <summary>
        /// This is a wrapper for the coroutine and checks it for errors
        /// </summary>
        private IEnumerator CoroutineWrapper(IEnumerator coroutine)
        {
            if (_inCoroutine) {
                Debug.LogWarning("Coroutine is currently running!");
                yield break;
            }

            _inCoroutine = true;
            
            CoroutineCatcher catcher = Routines.Catcher(coroutine);
            yield return catcher;

            _inCoroutine = false;
            
            if (catcher.HasThrown(out Exception exception)) {
                Debug.LogError(exception);
            }
        }
        
        
        private static readonly Regex Regex = new ("([A-Z])");
        private static string PrettifyName(string name)
        {
            return char.ToUpper(name[0]) + Regex.Replace(name[1..], " $&").Trim();
        }
    }
}