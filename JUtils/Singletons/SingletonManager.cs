using System.Collections.Generic;
using JUtils.Internal;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;



namespace JUtils.Singletons
{
    /// <summary>
    /// Centralized singleton to fix reference & hot reload issues issues 
    /// </summary>
    public class SingletonManager : MonoBehaviour
    {
        public static SingletonManager instance => JUtilsObject.instance.singletonManager;


        /// <summary>
        /// Get a singleton stored in the manager
        /// </summary>
        public static T GetSingleton<T>() where T : Object, ISingleton<T>
        {
            return instance._singletons.FirstOrDefault(x => x is T) as T;
        }


        /// <summary>
        /// Try get a singleton stored in the manager
        /// </summary>
        public static bool TryGetSingleton<T>(out T singleton) where T : Object, ISingleton<T>
        {
            singleton = GetSingleton<T>();
            return singleton != null;
        }


        /// <summary>
        /// Set a singleton reference
        /// </summary>
        public static bool SetSingleton<T>(ISingleton<T> singleton) where T : Object, ISingleton<T>
        {
           SingletonManager manager = instance;

           if (manager._singletons.Any(x => x is T)) {
               return false;
           }
           
           manager._singletons.Add(singleton as Object);
           return true;
        }


        /// <summary>
        /// Remove a singleton from all lists
        /// </summary>
        public static bool RemoveSingleton<T>(ISingleton<T> singleton) where T : Object, ISingleton<T>
        {
           return instance._singletons.Remove(singleton as Object);
        }

        //  Instance

        [SerializeField] private List<Object> _singletons = new ();
    } 
}