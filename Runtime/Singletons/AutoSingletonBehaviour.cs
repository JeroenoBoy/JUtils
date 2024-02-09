using JUtils.Internal;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Your simple and everyday singleton class that automatically instantiates if there is no instance yet
    /// </summary>
    public abstract class AutoSingletonBehaviour<T> : MonoBehaviour, ISingleton<T> where T : AutoSingletonBehaviour<T>
    {
        public static T instance {
            get {
                if (SingletonManager.TryGetSingleton(out T result)) return result;

                GameObject go = new(typeof(T).Name);
                result = go.AddComponent<T>();
                go.transform.parent = JUtilsObject.instance.transform;
                SingletonManager.SetSingleton(result);
                return result;
            }
        }

        public static bool exists => instance != null;

        private bool _doDestroy = true;


        protected virtual void Awake()
        {
            if (SingletonManager.SetSingleton(this)) return;

            Destroy(this);
            Debug.LogWarning("Instance already exists!", this);
        }


        protected virtual void OnDestroy()
        {
            if (!_doDestroy) return; // Prevents errors when exiting the game
            if (instance != this) return;
            SingletonManager.RemoveSingleton<T>();
        }


        private void OnApplicationQuit()
        {
            _doDestroy = false;
        }
    }
}