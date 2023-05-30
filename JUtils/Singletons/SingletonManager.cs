using System;
using JUtils.Internal;
using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// Centralized singleton to fix reference & hot reload issues issues 
    /// </summary>
    public class SingletonManager : MonoBehaviour
    {
        public static SingletonManager Instance => JUtilsObject.Instance.SingletonManager;


        /// <summary>
        /// Get a singleton from this class
        /// </summary>
        public static T GetSingleton<T>() where T : MonoBehaviour, ISingleton<T>
        {
           SingletonManager manager = Instance;
           
           Type type = typeof(T);
           if (manager._singletons.TryGetValue(type, out MonoBehaviour singleton)) {
               return singleton as T;
           }

           return default;
        }


        /// <summary>
        /// Set a singleton reference
        /// </summary>
        public static bool SetSingleton<T>(ISingleton<T> singleton) where T : MonoBehaviour, ISingleton<T>
        {
           SingletonManager manager = Instance;
           
           Type type = singleton.GetType();
           return manager._singletons.TryAdd(type, singleton as MonoBehaviour);
        }


        /// <summary>
        /// Remove a singleton from all lists
        /// </summary>
        public static bool RemoveSingleton<T>(ISingleton<T> singleton) where T : MonoBehaviour, ISingleton<T>
        {
           SingletonManager manager = Instance;
           
           Type type = singleton.GetType();

           if (!manager._singletons.TryGetValue(type, out MonoBehaviour foundSingleton)) return false;
           return foundSingleton == singleton && manager._singletons.Remove(type);
        }

        //  Instance

        private SerializableDictionary<Type, MonoBehaviour> _singletons;


        private void Awake()
        {
            _singletons = new SerializableDictionary<Type, MonoBehaviour>();
        }
    } 
}