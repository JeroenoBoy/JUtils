using System;
using JUtils.Extensions;
using JUtils.Internal;
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
        protected override void Awake()
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
    }
}