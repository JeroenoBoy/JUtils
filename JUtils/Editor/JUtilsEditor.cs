
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;



namespace JUtils.Editor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class JUtilsEditor : UnityEditor.Editor
    {
        private JUtilsEditorPostCallbackReceiver[] _postCallbackReceivers;
        
        public JUtilsEditor()
        {
            Type postCallbackType = typeof(JUtilsEditorPostCallbackReceiver);
            
            _postCallbackReceivers = Assembly.GetAssembly(postCallbackType).GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(postCallbackType))
                .Select(t => Activator.CreateInstance(t) as JUtilsEditorPostCallbackReceiver)
                .ToArray();
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
         
            if (_postCallbackReceivers is null) return;
            
            MonoBehaviour target = this.target as MonoBehaviour;

            
            foreach (JUtilsEditorPostCallbackReceiver receiver in _postCallbackReceivers) {
                receiver.PostCallback(target);
            }
        }
    }



    public abstract class JUtilsEditorPostCallbackReceiver
    {
        public abstract void PostCallback(MonoBehaviour target);
    }
#endif
}
