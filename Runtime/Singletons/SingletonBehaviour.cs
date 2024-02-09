using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Your simple and everyday singleton class
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour, ISingleton<T> where T : SingletonBehaviour<T>
    {
        public static T instance => SingletonManager.GetSingleton<T>();
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