using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Spawn an object using <see cref="ObjectPoolManager"/> after the game object was enalbed / created or the pool item was sent into action
    /// </summary>
    public sealed class SpawnPoolItemAfterEnabled : MonoBehaviour
    {
        [SerializeField] private PoolItem _poolItemToSpawn;

        private bool _hasPoolItem => _myPoolItem != null;
        private PoolItem _myPoolItem;

        private void Awake()
        {
            _myPoolItem = GetComponent<PoolItem>();
            if (_hasPoolItem) {
                _myPoolItem.onSpawn += HandlePoolItemSpawned;
            }
        }

        private void OnDestroy()
        {
            if (_hasPoolItem) {
                _myPoolItem.onSpawn -= HandlePoolItemSpawned;
            }
        }

        private void OnEnable()
        {
            if (_hasPoolItem) return;
            SpawnPoolItem();
        }

        private void SpawnPoolItem()
        {
            ObjectPoolManager.SpawnPoolItem(_poolItemToSpawn, transform.position, transform.rotation);
        }

        private void HandlePoolItemSpawned(PoolItem poolItem)
        {
            SpawnPoolItem();
        }
    }
}