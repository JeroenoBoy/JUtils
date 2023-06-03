using System;
using UnityEngine;



namespace JUtils.Components
{
    public class PoolItem : MonoBehaviour
    {
        public ObjectPool ObjectPool { get; internal set; }
        public bool       IsActive   { get; private set; }

        public event Action OnSpawn;
        public event Action OnDespawn;


        internal void Spawn()
        {
            gameObject.SetActive(true);
            IsActive = true;
            OnSpawn?.Invoke();
        }


        internal void Despawn()
        {
            OnDespawn?.Invoke();
            IsActive = false;
            gameObject.SetActive(false);
        }


        public void ReturnToPool()
        {
            if (!IsActive) return;
            ObjectPool.ReturnItem(this);
        }
    }
}
