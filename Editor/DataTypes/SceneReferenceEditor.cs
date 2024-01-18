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
    }
}