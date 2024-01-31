using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace JUtils
{
#if UNITY_EDITOR
    [StructLayout(LayoutKind.Auto)]
    public partial struct SceneReference : ISerializationCallbackReceiver
    {
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


        public void OnAfterDeserialize() { }
    }
#endif
}