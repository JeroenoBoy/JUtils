using System;
using JUtils.Extensions;
using JUtils.Internal;
using UnityEngine;



namespace JUtils.Singletons
{
    public class PersistentSingletonManager : SingletonBehaviour<PersistentSingletonManager>
    {
        public static bool exists => instance != null;
    
    
        public static PersistentSingletonManager persistentSingletonManager
        {
            get {
                if (instance) return instance;
    
                return JUtilsObject.GetOrAdd<PersistentSingletonManager>();
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
            DestroyImmediate(behaviour);
            
            T instance = gameObject.AddComponent<T>();
            if (instance is not MonoBehaviour copiedBehaviour) throw new Exception();
            copiedBehaviour.GetCopyOf(behaviour);
            
            if (instance is PersistentSingletonBehaviour<T> t) t.Init();
        }
    }
}
