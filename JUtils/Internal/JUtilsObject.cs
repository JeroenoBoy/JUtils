﻿using System;
using JUtils.Singletons;
using UnityEngine;



namespace JUtils.Internal
{
    /// <summary>
    /// Internal object used for <see cref="Coroutines"/> & <see cref="singletonManager"/>
    /// </summary>
    internal class JUtilsObject : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateInstance()
        {
            _instance = new GameObject("[JUtilsObject]").AddComponent<JUtilsObject>();
            DontDestroyOnLoad(_instance.gameObject);
        }
        
        
        private static  JUtilsObject _instance;
        internal static JUtilsObject instance => _instance ??= FindObjectOfType<JUtilsObject>();


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


        private SingletonManager _singletonManager;
        public  SingletonManager singletonManager => _singletonManager ??= GetOrAdd<SingletonManager>();
    }
}