using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A ScriptableObject version of a singleton
    /// </summary>
    /// <remarks>
    /// Place within the resources folder for it to automatically load!
    /// </remarks>
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject, ISingleton<T> where T : ScriptableObject, ISingleton<T>
    {
        private static T _instance;
        public static  T instance => GetInstance(true);

        public bool exists => GetInstance(false) != null;


        private static T GetInstance(bool logError)
        {
            if (_instance) return _instance;
            if (SingletonManager.TryGetSingleton(out T singleton)) return _instance = singleton;

            T[] objects = Resources.FindObjectsOfTypeAll<T>();
            if (objects.Length != 1) {
                if (logError) Debug.LogError(objects.Length == 0
                                                 ? $"No instance of {typeof(T)} exists in the resources folder" 
                                                 : $"Multiple instances of {typeof(T)} exist in the resources folder");
                return null;
            }

            SingletonManager.SetSingleton(objects[0]);
            return _instance = objects[0];
        }
    }
}
