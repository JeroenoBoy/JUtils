using System;
using System.Collections;
using System.Linq;
using JUtils.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;



namespace JUtils
{
    [System.Serializable]
#if UNITY_EDITOR
    public struct SceneReference : ISerializationCallbackReceiver
    {
#else
    public struct SceneReference
    {
#endif
        [SerializeField] private string _sceneName;
        [SerializeField] private string _scenePath;

        public string SceneName => _sceneName;
        public string ScenePath => _scenePath;

        public Scene Scene      => SceneManager.GetSceneByPath(_scenePath);
        public int   BuildIndex => Scene.buildIndex;


        public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(BuildIndex, mode);
        }


        public AsyncOperation LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(BuildIndex, mode);
        }


        public AsyncOperation UnloadSceneAsync()
        {
            return SceneManager.UnloadSceneAsync(BuildIndex);
        }
        
#if UNITY_EDITOR
        [SerializeField] private SceneAsset _sceneAsset;
        
        
        public void OnBeforeSerialize()
        {
            if (_sceneAsset == null) {
                _sceneName = string.Empty;
                _scenePath = string.Empty;
                return;
            }

            _sceneName = _sceneAsset.name;
            _scenePath = AssetDatabase.GetAssetPath(_sceneAsset);
        }


        public void OnAfterDeserialize()
        {
        }


        [CustomPropertyDrawer(typeof(SceneReference))]
        private class SceneReferenceEditor : PropertyDrawer
        {
            public enum ErrorType { Success, Null, BuildIndexMissing, PathMissing }

            private GUIStyle _backgroundStyle;
            
            
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                SerializedProperty sceneProperty = property.FindPropertyRelative(nameof(_sceneAsset));
                float height = EditorGUI.GetPropertyHeight(sceneProperty);
                
                return height + (HasError(sceneProperty.objectReferenceValue as SceneAsset, out _) ? height + 2 : 0 ) + 4;
            }


            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                GenerateStyles();
                GUI.Box(position, "", GUI.skin.box);

                position.width  -= 4;
                position.height -= 4;
                position.x      += 2;
                position.y      += 2;

                HasError(property.FindPropertyRelative(nameof(_sceneAsset)).objectReferenceValue as SceneAsset, out ErrorType errorType);
                Draw(position, property, label, errorType);
            }


            private void Draw(Rect position, SerializedProperty property, GUIContent label, ErrorType errorType)
            {
                SerializedProperty sceneProperty = property.FindPropertyRelative(nameof(_sceneAsset));

                position.height = EditorGUI.GetPropertyHeight(sceneProperty);
                EditorGUI.PropertyField(position, sceneProperty, label);

                SceneAsset sceneAsset = sceneProperty.objectReferenceValue as SceneAsset;
                string     path       = sceneAsset is not null ? AssetDatabase.GetAssetPath(sceneAsset) : string.Empty;

                property.FindPropertyRelative(nameof(_sceneName)).stringValue = sceneAsset ? path : string.Empty;
                property.FindPropertyRelative(nameof(_scenePath)).stringValue = path;
                
                position.y += position.height + 2;

                switch (errorType) {
                    case ErrorType.Success: break;
                    case ErrorType.Null:
                        EditorGUI.LabelField(position, "Reference is not set", _backgroundStyle);
                        break;
                    case ErrorType.BuildIndexMissing:
                        position.width -= 34;
                        EditorGUI.LabelField(position, "Scene has no build index", _backgroundStyle);

                        position.x     += position.width + 2;
                        position.width =  32;
                        Color prev = GUI.backgroundColor;
                        GUI.backgroundColor = Color.red;
                        if (GUI.Button(position, "Fix")) {
                            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Append(new EditorBuildSettingsScene(path, true)).ToArray();
                        }
                        GUI.backgroundColor = prev;
                        break;
                    case ErrorType.PathMissing:
                        EditorGUI.LabelField(position, "Could not resolve the path to this scene", _backgroundStyle);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }


            public static bool HasError(SceneAsset sceneAsset, out ErrorType errorType)
            {
                string path = sceneAsset is not null ? AssetDatabase.GetAssetPath(sceneAsset) : string.Empty;
                GUID guid = AssetDatabase.GUIDFromAssetPath(path);
                
                if (guid.Empty()) {
                    errorType = ErrorType.Null;
                    return true;
                }

                if (path == string.Empty) {
                    errorType = ErrorType.PathMissing;
                    return true;
                }

                int index = EditorBuildSettings.scenes.IndexOf(x => x.guid == guid);
                if (index < 0) {
                    errorType = ErrorType.BuildIndexMissing;
                    return true;
                }

                errorType = ErrorType.Success;
                return false;
            }


            private void GenerateStyles()
            {
                if (_backgroundStyle != null) {return;}
                
                Texture2D background = new Texture2D(1,1,TextureFormat.RGBAFloat,false);
                background.SetPixel(0,0, new Color(.015f,.0f,.01f, .2f));
                background.Apply();

                Texture2D hover = new Texture2D(1,1,TextureFormat.RGBAFloat, false);
                hover.SetPixel(0,0, new Color(.03f,.0f,.02f, .2f));
                hover.Apply();

                _backgroundStyle = new GUIStyle(GUI.skin.label)
                {
                    padding   = new RectOffset(2, 2, 2, 2),
                    margin    = new RectOffset(2, 2, 2, 2),
                    alignment = TextAnchor.MiddleCenter,
                    normal    = {background = background, textColor = new Color(1, .4f, .6f)}
                };
            }
        }
#endif
    }
}