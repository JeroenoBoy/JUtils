using System;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A behaviour that is used with the <see cref="ObjectPool"/>. Other behaviours can listen to the events this class sends.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PoolItem : MonoBehaviour
    {
        public ObjectPool objectPool { get; internal set; }
        public bool isActive { get; private set; }

        public event Action<PoolItem> onSpawn;
        public event Action<PoolItem> onDespawn;

        internal void Spawn()
        {
            gameObject.SetActive(true);
            isActive = true;
            onSpawn?.Invoke(this);
        }

        internal void Despawn()
        {
            onDespawn?.Invoke(this);
            isActive = false;
            gameObject.SetActive(false);
        }

        public void ReturnToPool()
        {
            if (!isActive) return;
            objectPool.ReturnItem(this);
        }

        private void OnDestroy()
        {
            if (objectPool == null) return;
            objectPool.PoolItemDestroyed(this);
        }
    }
}