using System.Linq;
using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// A ScriptableObject version of a singleton
    /// </summary>
    /// <remarks>
    /// Place within the resources folder for it to automatically load!
    /// </remarks>
    public abstract class SingletonScriptableObject<T> : ScriptableObject, ISingleton<T> where T : ScriptableObject, ISingleton<T>
    {
        private static T _instance;
        public static T instance
        {
            get {
                if (_instance) return _instance;
                if (SingletonManager.TryGetSingleton(out T singleton)) return _instance = singleton;

                T[] objects = Resources.FindObjectsOfTypeAll<T>();
                if (objects.Length != 1) {
                    Debug.LogError(objects.Length == 0 ? $"No instance of {nameof(T)} exists in the resources folder" : $"Multiple instances of {nameof(T)} exist in the resources folder");
                    return null;
                }

                SingletonManager.SetSingleton(objects[0]);
                return _instance = objects[0];
            }
        }
    }
}
