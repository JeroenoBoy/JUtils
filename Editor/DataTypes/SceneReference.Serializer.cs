using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace JUtils
{
    [StructLayout(LayoutKind.Auto)]
    public partial struct SceneReference
    {
        [SerializeField] internal SceneAsset _sceneAsset;
        
        
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
    }
}