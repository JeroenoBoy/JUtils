using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// Your simple and everyday singleton class
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour, ISingleton<T>
        where T : MonoBehaviour, ISingleton<T>
    {
        public static T instance => SingletonManager.GetSingleton<T>();


        protected virtual void OnEnable()
        {
            if (SingletonManager.SetSingleton(this)) return;
            
            Destroy(this);
            Debug.LogWarning("Instance already exists!");
        }


        protected virtual void OnDisable()
        {
            SingletonManager.RemoveSingleton(this);
        }
    }

    
    
    /// <summary>
    /// Centralized singleton to fix reference & hot reload issues issues 
    /// </summary>
    public class SingletonManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static SingletonManager _instance;
        public static SingletonManager instance
        {
            get {
                if (_instance != null) return _instance;
                
                new GameObject("[JUtils Singleton Manager]").AddComponent<SingletonManager>();
                return _instance;
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
            if (manager._fastLookup.ContainsKey(type)) return manager._fastLookup[type] as T;
            
            //  Finding reference
            
            for (int i = 0; i < manager._singletons.Count; i++) {
                if (manager._singletons[i] is not T singleton) continue;
                
                manager._fastLookup.Add(type, singleton);
                return singleton;
            }
            
            //  Error

            throw new ArgumentException($"Singleton of type {type} does not exist");
        }
        

        internal static bool SetSingleton<T>(SingletonBehaviour<T> singleton) where T : MonoBehaviour, ISingleton<T>
        {
            SingletonManager manager = instance;
            
            Type type = singleton.GetType();
            MonoBehaviour behaviour = manager._singletons.FirstOrDefault(s => s is T);
            if (behaviour != null) return singleton == behaviour;

            manager._singletons.Add(singleton);
            manager._fastLookup.Add(type, singleton);
            return true;
        }
        

        internal static void RemoveSingleton<T>(SingletonBehaviour<T> singleton) where T : MonoBehaviour, ISingleton<T>
        {
            if (_instance == null) return;
            SingletonManager manager = instance;
            
            Type type = singleton.GetType();
            if (manager._fastLookup.ContainsKey(type)) manager._fastLookup.Remove(type);

            manager._singletons.Remove(singleton);
;       }
        
        
        //  Instance


        [SerializeField] private List<MonoBehaviour> _singletons;
        private Dictionary<Type, MonoBehaviour> _fastLookup;


        private void OnEnable()
        {
            if (_instance == null) {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this) {
                Debug.LogError("SingletonManager already exists!");
                return;
            }
            
            _fastLookup ??= new ();
            _singletons ??= new ();
        }


        private void OnDisable()
        {
            if (instance == this) _instance = null;
        }
        
        
        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            _instance = this;
            _fastLookup ??= new ();
            _singletons ??= new ();
        }
    }
}