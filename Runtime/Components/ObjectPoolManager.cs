using System.Linq;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Makes it easier to work with object pools
    /// </summary>
    public class ObjectPoolManager : AutoSingletonBehaviour<ObjectPoolManager>
    {
        private SerializableDictionary<PoolItem, ObjectPool> _prefabPoolMap = new();


        /// <summary>
        /// Spawns a pooled item from the prefab
        /// </summary>
        public PoolItem SpawnPoolItem(PoolItem template, Vector3? position = null, Quaternion? rotation = null, Vector3? localScale = null, Transform parent = null)
        {
            PoolItem poolItem = GetObjectPool(template).SpawnItem();
            if (parent != null) poolItem.transform.parent = parent;
            if (position != null) poolItem.transform.position = position.Value;
            if (rotation != null) poolItem.transform.rotation = rotation.Value;
            if (localScale != null) poolItem.transform.localScale = localScale.Value;
            return poolItem;
        }


        /// <summary>
        /// Get or create an object pool for the given prefab
        /// </summary>
        public ObjectPool GetObjectPool(PoolItem template)
        {
            if (!_prefabPoolMap.TryGetValue(template, out ObjectPool pool)) {
                pool = CreateObjectPool(template);
            }

            return pool;
        }


        /// <summary>
        /// Destroys the object pool assigned to the prefab
        /// </summary>
        public bool DestroyObjectPool(PoolItem template, bool cleanObjects = true)
        {
            if (!_prefabPoolMap.TryGetValue(template, out ObjectPool pool)) return false;

            if (cleanObjects) {
                foreach (PoolItem poolItem in pool.activePoolItems) {
                    Destroy(poolItem.gameObject);
                }
            } else if (pool.freePoolItems.Count() != pool.activePoolItems.Count()) {
                Debug.LogError("Cannot destroy object pool with filled items", template);
            }

            Destroy(pool);
            _prefabPoolMap.Remove(template);
            return true;
        }


        private ObjectPool CreateObjectPool(PoolItem template)
        {
            GameObject obj = new($"Pool: {template.name}");
            ObjectPool pool = obj.AddComponent<ObjectPool>();
            pool.SetTemplate(template);
            _prefabPoolMap[template] = pool;
            obj.transform.parent = transform;
            obj.transform.Reset();
            return pool;
        }
    }
}