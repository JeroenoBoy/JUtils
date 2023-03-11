using System;
using System.Collections.Generic;
using System.Linq;
using JUtils.Internal;
using JUtils.Singletons;
using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// Centralized singleton to fix reference & hot reload issues issues 
    /// </summary>
    public class SingletonManager : MonoBehaviour
    {
        private static SingletonManager _instance;
        public static SingletonManager instance
        {
           get {
               if (_instance != null) return _instance;
               return _instance = JUtilsObject.GetOrAdd<SingletonManager>();
           }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static T GetSingleton<T>() where T : MonoBehaviour, ISingleton<T>
        {
           SingletonManager manager = instance;
           
           Type type = typeof(T);
           if (manager._singletons.TryGetValue(type, out MonoBehaviour singleton)) {
               return singleton as T;
           }

           return null;
        }


        public static bool SetSingleton<T>(SingletonBehaviour<T> singleton) where T : MonoBehaviour, ISingleton<T>
        {
           SingletonManager manager = instance;
           
           Type type = singleton.GetType();
           return manager._singletons.TryAdd(type, singleton);
        }


        public static bool RemoveSingleton<T>(SingletonBehaviour<T> singleton) where T : MonoBehaviour, ISingleton<T>
        {
           SingletonManager manager = instance;
           
           Type type = singleton.GetType();

           if (!manager._singletons.TryGetValue(type, out MonoBehaviour foundSingleton)) return false;
           return foundSingleton == singleton && manager._singletons.Remove(type);
        }


        //  Instance


        private SerializableDictionary<Type, MonoBehaviour> _singletons;


        private void Awake()
        {
           if (_instance == null) {
               _instance = this;
               DontDestroyOnLoad(gameObject);
           }
           else if (_instance != this) {
               Debug.LogError("SingletonManager already exists!");
               return;
           }

           _singletons = new SerializableDictionary<Type, MonoBehaviour>();
        }


        private void OnDestroy()
        {
           if (instance == this) _instance = null;
        }
    } 
}