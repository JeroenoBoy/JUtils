using System;
using System.Collections.Generic;
using JUtils.Internal;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;



namespace JUtils.Singletons
{
    /// <summary>
    /// Centralized singleton to fix reference & hot reload issues issues 
    /// </summary>
    public class SingletonManager : MonoBehaviour
    {
        public static SingletonManager instance => JUtilsObject.instance.singletonManager;


        /// <summary>
        /// Get a singleton from this class
        /// </summary>
        public static T GetSingleton<T>() where T : MonoBehaviour, ISingleton<T>
        {
            return instance._singletons.FirstOrDefault(x => x is T) as T;
        }


        /// <summary>
        /// Set a singleton reference
        /// </summary>
        public static bool SetSingleton<T>(ISingleton<T> singleton) where T : MonoBehaviour, ISingleton<T>
        {
           SingletonManager manager = instance;

           if (manager._singletons.Any(x => x is T)) {
               return false;
           }
           
           manager._singletons.Add(singleton as MonoBehaviour);
           return true;
        }


        /// <summary>
        /// Remove a singleton from all lists
        /// </summary>
        public static bool RemoveSingleton<T>(ISingleton<T> singleton) where T : MonoBehaviour, ISingleton<T>
        {
           return instance._singletons.Remove(singleton as MonoBehaviour);
        }

        //  Instance

        [SerializeField] private List<MonoBehaviour> _singletons = new ();
    } 
}