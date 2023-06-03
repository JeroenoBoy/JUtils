using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// Your simple and everyday singleton class
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour, ISingleton<T> where T : MonoBehaviour, ISingleton<T>
    {
        private static T _instance;
        public static  T Instance => _instance ??= SingletonManager.GetSingleton<T>();


        protected virtual void Awake()
        {
            if (SingletonManager.SetSingleton(this)) return;
            
            Destroy(this);
            Debug.LogWarning("Instance already exists!");
        }


        protected virtual void OnDestroy()
        {
            SingletonManager.RemoveSingleton(this);
        }
    }
}
