using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JUtils.Editor
{
    internal static class PlayModeChangeDetector
    {
        private static (Type type, ResourcePlayModeChangeCallbackReceiverAttribute attribute)[] _attributes;

        [InitializeOnLoadMethod]
        private static void RegisterPlayModeChangeDetection()
        {
            _attributes = AssemblyJUtils.GetAllTypes()
                .Select(it => (type: it, attribute: it.GetCustomAttribute<ResourcePlayModeChangeCallbackReceiverAttribute>(true)))
                .Where(it => it.attribute != null)
                .ToArray();

            EditorApplication.playModeStateChanged += HandlePlayModeChanged;
        }


        private static void HandlePlayModeChanged(PlayModeStateChange stateChange)
        {
            if (stateChange is not (PlayModeStateChange.EnteredEditMode or PlayModeStateChange.ExitingEditMode)) return;
            string memberName = stateChange == PlayModeStateChange.ExitingEditMode
                ? nameof(IResourcePlayModeChangeCallbackReceiver.OnPlayModeEnter)
                : nameof(IResourcePlayModeChangeCallbackReceiver.OnPlayModeExit);

            foreach ((Type type, ResourcePlayModeChangeCallbackReceiverAttribute attribute) in _attributes) {
                MethodInfo method = type.GetMethod(memberName);
                if (method == null) continue;
                foreach (Object obj in Resources.LoadAll(attribute.folder, type)) {
                    method.Invoke(obj, Array.Empty<object>());
                }
            }
        }
    }
}