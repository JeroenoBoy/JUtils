#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;



namespace JUtils.Editor
{
    /// <summary>
    /// A small debug window which allows for setting VSync, TimeScale and a FPS limit
    /// </summary>
    public class DebugToolsWindow : EditorWindow
    {
        private bool _isOverriding;
        private int  _fps;


        private bool _vSync
        {
            get => QualitySettings.vSyncCount > 0;
            set => QualitySettings.vSyncCount = value ? 1 : 0;
        }
        
        
        [MenuItem("Window/Debug")] 
        public static void Initialize()
        {
            GetWindow<DebugToolsWindow>("Debug Tools");
        }


        private void OnGUI()
        {
            if (!Application.isPlaying) {
                GUILayout.Label("Only available in play mode");
                return;
            }
        
            GUIStyle textStyle = new GUIStyle(GUI.skin.label)
            {
                fixedWidth = Mathf.Min(Screen.width*.5f, 100)
            };
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Override FPS", textStyle);
            _isOverriding = EditorGUILayout.Toggle(_isOverriding);
            GUILayout.EndHorizontal();

            if (_isOverriding) {
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("FPS", textStyle);
                _fps = EditorGUILayout.IntField(_fps);
                EditorGUILayout.EndHorizontal();

                _vSync = false;
                Application.targetFrameRate = _fps;
            }
            else {
                Application.targetFrameRate = int.MaxValue;
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("VSync", textStyle);
                _vSync = EditorGUILayout.Toggle(_vSync);
                GUILayout.EndHorizontal();
            }
            
            
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Time Scale", textStyle);
            
            float value = EditorGUILayout.FloatField(Time.timeScale);
            value = Mathf.Clamp(value, 0, 100);
            Time.timeScale = value;
            
            GUILayout.EndHorizontal();
        }
    }
}
#endif
