using System;
using JUtils.Singletons;
using UnityEngine;



namespace JUtils.Internal
{
    internal class JUtilsObject : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateInstance()
        {
            _instance = new GameObject("[JUtilsObject]").AddComponent<JUtilsObject>();
            DontDestroyOnLoad(_instance.gameObject);
        }
        
        
        private static  JUtilsObject _instance;
        internal static JUtilsObject Instance => _instance ??= FindObjectOfType<JUtilsObject>();


        internal static T Add<T>() where T : Component
        {
            return Instance.gameObject.AddComponent<T>();
        }


        internal static T Get<T>() where T : Component
        {
            return Instance.GetComponent<T>();
        }


        internal static T GetOrAdd<T>() where T : Component
        {
            return Instance.GetComponent<T>() ?? Add<T>();
        }


        private SingletonManager _singletonManager;
        public  SingletonManager SingletonManager => _singletonManager ??= GetOrAdd<SingletonManager>();
    }
}