using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.Editor
{
    public class DebugTools : EditorWindow
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
            GetWindow<DebugTools>("Debug Tools");
        }


        private void OnGUI()
        {
            if (!Application.isPlaying) {
                GUILayout.Label("Only available in play mode");
                return;
            }
        
            var textStyle = new GUIStyle(GUI.skin.label)
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
            
            var value = EditorGUILayout.FloatField(Time.timeScale);
            value = Mathf.Clamp(value, 0, 100);
            Time.timeScale = value;
            
            GUILayout.EndHorizontal();
        }
    }
}
