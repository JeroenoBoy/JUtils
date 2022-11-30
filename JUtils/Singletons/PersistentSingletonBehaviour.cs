using System;
using JUtils.Extensions;
using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// When the scene starts, this component removes itself from the Gameobject and adds a new instance of itself to the PersistentSingletonManager
    /// If you add the Persistent Singleton Manager to a gameobject, it won't auto create itself.
    ///
    /// Recommended to override OnAwake instead of using Awake to prevent duplicate awake calls. 
    /// </summary>
    public abstract class PersistentSingletonBehaviour<T> : SingletonBehaviour<T>
        where T : MonoBehaviour, ISingleton<T>
    {
        protected bool attachedToManager { get; private set; }


        /// <summary>
        /// The main culprit to the chaos
        /// </summary>
        protected override void OnEnable()
        {
            if (!SingletonManager.SetSingleton(this)) {
                Destroy(this);
                Debug.LogWarning("Instance already exists, destroying current instance");
                return;
            }

            PersistentSingletonManager persistentSingletonManager = PersistentSingletonManager.persistentSingletonManager;
            persistentSingletonManager.TryAddComponent(this as T);
        }


        internal void Init()
        {
            if (attachedToManager) return;
            attachedToManager = true;
            OnAwake();
        }
        

        /// <summary>
        /// Use this to prevent duplicate Awake calls
        /// </summary>
        protected abstract void OnAwake();


        /// <summary>
        /// Removes itself as the instance
        /// </summary>
        protected virtual void OnDisable()
        {
            SingletonManager.RemoveSingleton(this);
        }
    }
        
        
        
        
    public class PersistentSingletonManager : SingletonBehaviour<PersistentSingletonManager>
    {
        public static bool exists => instance != null;
    
    
        public static PersistentSingletonManager persistentSingletonManager
        {
            get {
                if (instance) return instance;
    
                SingletonManager.instance.gameObject.AddComponent<PersistentSingletonManager>();
                return instance;
            }
        }


        public bool TryAddComponent<T>(T behaviour)
            where T : MonoBehaviour, ISingleton<T>
        {
            if (this.HasComponent<T>()) return false;
            AddComponent(behaviour);
            
            
            return true;
        }


        public void AddComponent<T>(T behaviour)
            where T : MonoBehaviour, ISingleton<T>
        {
            Destroy(behaviour);
            
            T instance = gameObject.AddComponent<T>();
            if (instance is not MonoBehaviour copiedBehaviour) throw new Exception();
            copiedBehaviour.GetCopyOf(behaviour);
            
            if (instance is PersistentSingletonBehaviour<T> t) t.Init();
        }
    }



}