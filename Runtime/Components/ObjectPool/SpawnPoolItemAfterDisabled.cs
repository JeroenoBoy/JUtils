using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Spawn an object using <see cref="ObjectPoolManager"/> after the game object was disabled / destroyed or the pool item was sent back to the object pool
    /// </summary>
    public sealed class SpawnPoolItemAfterDestroyed : MonoBehaviour
    {
        [SerializeField] private PoolItem _poolItemToSpawn;

        private bool _hasPoolItem => _myPoolItem != null;
        private PoolItem _myPoolItem;

        private void Awake()
        {
            _myPoolItem = GetComponent<PoolItem>();
            if (_hasPoolItem) {
                _myPoolItem.onDespawn += HandlePoolItemDespawned;
            }
        }

        private void OnDestroy()
        {
            if (_hasPoolItem) {
                _myPoolItem.onDespawn -= HandlePoolItemDespawned;
            }
        }

        private void OnDisable()
        {
            if (_hasPoolItem) return;
            SpawnPoolItem();
        }

        private void SpawnPoolItem()
        {
            ObjectPoolManager.SpawnPoolItem(_poolItemToSpawn, transform.position, transform.rotation);
        }

        private void HandlePoolItemDespawned(PoolItem poolItem)
        {
            SpawnPoolItem();
        }
    }
}