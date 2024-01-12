using JUtils.Internal;
using UnityEngine;
using Object = UnityEngine.Object;


namespace JUtils
{
    /// <summary>
    /// Centralized singleton to fix reference & hot reload issues issues 
    /// </summary>
    public sealed class SingletonManager : MonoBehaviour
    {
        public static SingletonManager instance => JUtilsObject.instance.singletonManager;

        [Uneditable]
        [SerializeField] private SerializableDictionary<SerializableType, Object> _singletons = new();


        /// <summary>
        /// Get a singleton stored in the manager
        /// </summary>
        public static T GetSingleton<T>() where T : Object, ISingleton<T>
        {
            TryGetSingleton(out T result);
            return result;
        }


        /// <summary>
        /// Try get a singleton stored in the manager
        /// </summary>
        public static bool TryGetSingleton<T>(out T singleton) where T : Object, ISingleton<T>
        {
            if (instance._singletons.TryGetValue(new SerializableType(typeof(T)), out Object result)) {
                singleton = result as T;
                return true;
            }

            singleton = null;
            return false;
        }


        /// <summary>
        /// Set a singleton reference
        /// </summary>
        public static bool SetSingleton<T>(ISingleton<T> singleton) where T : Object, ISingleton<T>
        {
            SingletonManager manager = instance;
            SerializableType type = new(typeof(T));

            if (manager._singletons.ContainsKey(type)) {
                return false;
            }

            manager._singletons.Add(type, singleton as Object);
            return true;
        }


        /// <summary>
        /// Remove a singleton from all lists
        /// </summary>
        public static bool RemoveSingleton<T>() where T : Object, ISingleton<T>
        {
            return instance._singletons.Remove(new SerializableType(typeof(T)));
        }
    }
}