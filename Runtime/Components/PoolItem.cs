using System;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A behaviour that is used with the <see cref="objectPool"/>. Other behaviours can listen to the events this class sends.
    /// </summary>
    public class PoolItem : MonoBehaviour
    {
        public ObjectPool objectPool { get; internal set; }
        public bool       isActive   { get; private set; }

        public event Action onSpawn;
        public event Action onDespawn;


        internal void Spawn()
        {
            gameObject.SetActive(true);
            isActive = true;
            onSpawn?.Invoke();
        }


        internal void Despawn()
        {
            onDespawn?.Invoke();
            isActive = false;
            gameObject.SetActive(false);
        }


        public void ReturnToPool()
        {
            if (!isActive) return;
            objectPool.ReturnItem(this);
        }
    }
}
