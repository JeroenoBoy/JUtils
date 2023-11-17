using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ObjectField = UnityEditor.Search.ObjectField;

namespace JUtils.Editor
{
    
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceEditor : PropertyDrawer
    {
        private const string PROPERTY_NAME_SCENEASSET = "_sceneAsset";
        private const string PROPERTY_NAME_SCENENAME = "_sceneName";
        private const string PROPERTY_NAME_SCENEPATH = "_scenePath";

        private const string ERROR_NULL = "Reference is not set";
        private const string ERROR_BUILD_INDEX_MISSING = "Build index was not found";
        private const string ERROR_PATH_MISSING = "Could not resolve the path to this scene";
        
        
        public enum ErrorType { Success, Null, BuildIndexMissing, PathMissing }

        
        private SceneReference _sceneRef;
        private SceneAsset _sceneAsset;
        
        private ObjectField _objectField;
        private VisualElement _errorElement;
        private TextElement _errorText;
        private Button _buttonElement;

        private SerializedProperty _sceneAssetProperty;
        
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _sceneAssetProperty = property.FindPropertyRelative(PROPERTY_NAME_SCENEASSET);
            
            VisualElement root = new() {
                style = {
                    backgroundColor = new Color(.19f, .19f, .19f),
                    paddingTop = 2f,
                    paddingBottom = 2f,
                    paddingLeft = 2f,
                    paddingRight = 2f,
                    borderTopLeftRadius = 4f,
                    borderTopRightRadius = 4f,
                    borderBottomLeftRadius = 4f,
                    borderBottomRightRadius = 4f
                }
            };

            _objectField = new ObjectField(property.displayName);
            _objectField.BindProperty(_sceneAssetProperty);
            _objectField.RegisterValueChangedCallback(_ => UpdateMessage());
            
            _errorElement = new VisualElement();
            _errorText = new TextElement();
            _buttonElement = new Button();

            _errorElement.style.marginLeft = 3;
            _errorElement.style.marginRight = 2;
            _errorElement.style.marginTop = 2;
            _errorElement.style.marginBottom = 2;
            
            _buttonElement.clicked += HandleButtonClicked;
            _errorElement.style.flexDirection = FlexDirection.Row;
            _errorText.style.flexGrow = 1;
                
            _buttonElement.style.display = DisplayStyle.None;
            _errorElement.style.display = DisplayStyle.None;
            
            
            root.Add(_objectField);
            root.Add(_errorElement);
            
            _errorElement.Add(_errorText);
            _errorElement.Add(_buttonElement);
            UpdateMessage();
            return root;
        }


        private void UpdateMessage()
        {
            SceneAsset sceneAsset = GetSceneAsset();
            ErrorType error = GetError(sceneAsset);

            switch (error) {
                case ErrorType.Success:
                    break;
                case ErrorType.Null:
                    _errorText.text = "Scene was not found";
                    break;
                case ErrorType.BuildIndexMissing:
                    _errorText.text = "Scene has no build index";
                    _buttonElement.text = "Fix";
                    break;
                case ErrorType.PathMissing:
                    _errorText.text = "Path to this scene was not found";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _errorElement.style.display = error == ErrorType.Success ? DisplayStyle.None : DisplayStyle.Flex;
            _buttonElement.style.display = error == ErrorType.BuildIndexMissing ? DisplayStyle.Flex : DisplayStyle.None;
        }


        private ErrorType GetError([CanBeNull] SceneAsset sceneAsset)
        {
            string path = sceneAsset is not null ? AssetDatabase.GetAssetPath(sceneAsset) : string.Empty;
            GUID guid = AssetDatabase.GUIDFromAssetPath(path);

            if (guid.Empty()) return ErrorType.Null;
            if (path == string.Empty) return ErrorType.PathMissing;

            int index = EditorBuildSettings.scenes.IndexOf(x => x.guid == guid);
            if (index < 0) return ErrorType.BuildIndexMissing;

            return ErrorType.Success;
        }


        private void HandleButtonClicked()
        {
            SceneAsset asset = GetSceneAsset();
            if (GetError(asset) != ErrorType.BuildIndexMissing) return;

            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Append(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(asset), true)).ToArray();
            UpdateMessage();
        }


        [CanBeNull]
        private SceneAsset GetSceneAsset() => _sceneAssetProperty.objectReferenceValue as SceneAsset;


        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        // {
        //     SerializedProperty sceneProperty = property.FindPropertyRelative(PROPERTY_NAME_SCENEASSET);
        //     float height = EditorGUI.GetPropertyHeight(sceneProperty);
        //     
        //     return height + (HasError(sceneProperty.objectReferenceValue as SceneAsset, out _) ? height + 2 : 0 ) + 4;
        // }
        //
        //
        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        // {
        //     GenerateStyles();
        //     GUI.Box(position, "", GUI.skin.box);
        //
        //     position.width  -= 4;
        //     position.height -= 4;
        //     position.x      += 2;
        //     position.y      += 2;
        //
        //     HasError(property.FindPropertyRelative(PROPERTY_NAME_SCENEASSET).objectReferenceValue as SceneAsset, out ErrorType errorType);
        //     Draw(position, property, label, errorType);
        // }
        //
        //
        // private void Draw(Rect position, SerializedProperty property, GUIContent label, ErrorType errorType)
        // {
        //     SerializedProperty sceneProperty = property.FindPropertyRelative(PROPERTY_NAME_SCENEASSET);
        //
        //     position.height = EditorGUI.GetPropertyHeight(sceneProperty);
        //     EditorGUI.PropertyField(position, sceneProperty, label);
        //
        //     SceneAsset sceneAsset = sceneProperty.objectReferenceValue as SceneAsset;
        //     string     path       = sceneAsset is not null ? AssetDatabase.GetAssetPath(sceneAsset) : string.Empty;
        //
        //     property.FindPropertyRelative(PROPERTY_NAME_SCENENAME).stringValue = sceneAsset ? path : string.Empty;
        //     property.FindPropertyRelative(PROPERTY_NAME_SCENEPATH).stringValue = path;
        //     
        //     position.y += position.height + 2;
        //
        //     switch (errorType) {
        //         case ErrorType.Success: break;
        //         case ErrorType.Null:
        //             EditorGUI.LabelField(position, "Reference is not set", _backgroundStyle);
        //             break;
        //         case ErrorType.BuildIndexMissing:
        //             position.width -= 34;
        //             EditorGUI.LabelField(position, "Scene is not present in build settings", _backgroundStyle);
        //
        //             position.x     += position.width + 2;
        //             position.width =  32;
        //             Color prev = GUI.backgroundColor;
        //             GUI.backgroundColor = Color.red;
        //             if (GUI.Button(position, "Fix")) {
        //                 EditorBuildSettings.scenes = EditorBuildSettings.scenes.Append(new EditorBuildSettingsScene(path, true)).ToArray();
        //             }
        //             GUI.backgroundColor = prev;
        //             break;
        //         case ErrorType.PathMissing:
        //             EditorGUI.LabelField(position, "Could not resolve the path to this scene", _backgroundStyle);
        //             break;
        //         default:
        //             throw new NotImplementedException();
        //     }
        // }
        //
        //
        // public static bool HasError(SceneAsset sceneAsset, out ErrorType errorType)
        // {
        //     string path = sceneAsset is not null ? AssetDatabase.GetAssetPath(sceneAsset) : string.Empty;
        //     GUID guid = AssetDatabase.GUIDFromAssetPath(path);
        //     
        //     if (guid.Empty()) {
        //         errorType = ErrorType.Null;
        //         return true;
        //     }
        //
        //     if (path == string.Empty) {
        //         errorType = ErrorType.PathMissing;
        //         return true;
        //     }
        //
        //     int index = EditorBuildSettings.scenes.IndexOf(x => x.guid == guid);
        //     if (index < 0) {
        //         errorType = ErrorType.BuildIndexMissing;
        //         return true;
        //     }
        //
        //     errorType = ErrorType.Success;
        //     return false;
        // }
        //
        //
        // private void GenerateStyles()
        // {
        //     if (_backgroundStyle != null) {return;}
        //     
        //     Texture2D background = new(1,1,TextureFormat.RGBAFloat,false);
        //     background.SetPixel(0,0, new Color(.015f,.0f,.01f, .2f));
        //     background.Apply();
        //
        //     Texture2D hover = new(1,1,TextureFormat.RGBAFloat, false);
        //     hover.SetPixel(0,0, new Color(.03f,.0f,.02f, .2f));
        //     hover.Apply();
        //
        //     _backgroundStyle = new GUIStyle(GUI.skin.label)
        //     {
        //         padding   = new RectOffset(2, 2, 2, 2),
        //         margin    = new RectOffset(2, 2, 2, 2),
        //         alignment = TextAnchor.MiddleCenter,
        //         normal    = {background = background, textColor = new Color(1, .4f, .6f)}
        //     };
        // }
    }
}