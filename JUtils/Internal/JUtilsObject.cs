using System;
using UnityEngine;



namespace JUtils.Internal
{
    internal class JUtilsObject : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static JUtilsObject _instance;
        internal static JUtilsObject instance
        {
            get {
                if (_instance != null) return _instance;
                return _instance = new GameObject("[JUtils Components]").AddComponent<JUtilsObject>();
            }
        }


        internal static T Add<T>() where T : Component
        {
            return instance.gameObject.AddComponent<T>();
        }


        internal static T Get<T>() where T : Component
        {
            return instance.GetComponent<T>();
        }


        internal static T GetOrAdd<T>() where T : Component
        {
            return instance.GetComponent<T>() ?? Add<T>();
        }
        

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }


        public void OnBeforeSerialize() {}
        public void OnAfterDeserialize()
        {
            _instance = this;
        }
    }
}

