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
    public abstract class PersistentSingletonBehaviour<T> : MonoBehaviour, ISingleton<T>, ISerializationCallbackReceiver
        where T : MonoBehaviour, ISingleton<T>
    {
        public static T instance { get; private set; }
        protected bool attachedToManager { get; private set; }


        /// <summary>
        /// The main culprit to the chaos
        /// </summary>
        protected virtual void OnEnable()
        {
            if (instance && instance != this) {
                Destroy(this);
                Debug.LogWarning("Instance already exists, destroying current instance");
                return;
            }
            
            instance = this as T;

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
            if (instance == this) instance = null;
        }


        //  Serialization management
        

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            if (instance && instance != this) {
                try {
                    Debug.LogWarning("Instance already exists, destroying current instance");
                }
                catch {
                    Type type = GetType();
                    Debug.LogWarning($"Multiple instances of \"{type.Namespace}.{type.Name}\" exist!");
                }
                return;
            }

            instance = this as T;
        }
    }
    
        
    public class PersistentSingletonManager : SingletonBehaviour<PersistentSingletonManager>
    {
        public static bool exists => instance != null;
    
    
        public static PersistentSingletonManager persistentSingletonManager
        {
            get {
                if (instance) return instance;
    
                GameObject obj = new ("JUtils Persistent Singletons");
                obj.AddComponent<PersistentSingletonManager>();
                DontDestroyOnLoad(obj);
                return instance;
            }
        }


        public bool HasComponent<T>()
            where T : MonoBehaviour, ISingleton<T>
        {
            return gameObject.GetComponent<T>() != null;
        }


        public bool TryAddComponent<T>(T behaviour)
            where T : MonoBehaviour, ISingleton<T>
        {
            if (HasComponent<T>()) return false;
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