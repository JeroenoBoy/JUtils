using UnityEngine;

namespace JUtils
{
    public class ObjectPoolManager : SingletonBehaviour<ObjectPoolManager>
    {
        [SerializeField] private bool _autoCreateObjectPools = false;
            
        private SerializableDictionary<GameObject, ObjectPool> _prefabPoolMap = new();
        
        
    }
}