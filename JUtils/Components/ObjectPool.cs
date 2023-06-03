using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;



namespace JUtils.Components
{
    public sealed class ObjectPool : MonoBehaviour
    {
        [SerializeField] private PoolItem template;
        [SerializeField] private int prefill = 10;
        [SerializeField] private int maxSize = -1;
        
        [SerializeField] private bool autoExpand = true;
        [SerializeField] private int autoExpandAmount = 5;
        
        private PoolItem[] poolItems;
        private Queue<PoolItem> freeItems;


        public PoolItem SpawnItem()
        {
            return SpawnItem(Vector3.zero, Quaternion.identity);
        }


        public PoolItem SpawnItem(Vector3 localPosition) => SpawnItem(localPosition, Quaternion.identity);
        public PoolItem SpawnItem(Vector3 localPosition, Transform parent) => SpawnItem(localPosition, Quaternion.identity, parent);
        public PoolItem SpawnItem(Transform parent) => SpawnItem(Vector3.zero, Quaternion.identity, parent);
        public PoolItem SpawnItem(Vector3 localPosition, Quaternion localRotation, Transform parent = null)
        {
            if (!TryGetItem(out PoolItem item)) return null;
            Transform transform = item.transform;
            transform.parent        = parent;
            transform.localPosition = localPosition;
            transform.rotation      = localRotation;
            return item;
        }
        
        
        public bool TryGetItem(out PoolItem item)
        {
            if (freeItems.Count == 0 && (!autoExpand || AddItems() <= 0)) {
                Debug.LogWarning("Tried to get item from empty pool");
                item = null;
                return false;
            }

            item = freeItems.Dequeue();
            item.Spawn();

            return true;
        }
        
        
        public int AddItems(int amount = -1)
        {
            if (amount <= 0) amount = autoExpandAmount;
            int newSize = maxSize > 0 ? Mathf.Max(poolItems.Length, amount + poolItems.Length) : amount + poolItems.Length;
            amount = newSize - poolItems.Length;

            if (newSize == poolItems.Length) {
                return 0;
            }
            
            Array.Resize(ref poolItems, newSize);

            while (amount --> 0) {
                PoolItem item = Instantiate(template, Vector3.zero, Quaternion.identity, transform);
                item.ObjectPool = this;
                item.gameObject.SetActive(false);
                poolItems[newSize - amount - 1] = item;
                freeItems.Enqueue(item);
            }
            
            return amount;
        }


        internal bool ReturnItem(PoolItem item)
        {
            if (!poolItems.Contains(item) || freeItems.Contains(item)) {
                return false;
            }

            item.Despawn();
            Transform transform = item.transform;
            transform.parent        = this.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            freeItems.Enqueue(item);
            
            return true;
        }


        private void Awake()
        {
            if (template == null) {
                Debug.LogError("No template has been set!", this);
                return;
            }

            if (template.transform.parent != transform) return;
            
            template.gameObject.SetActive(false);
            template.ObjectPool = this;
        }
        

        private void Start()
        {
            if (!template) return;
            
            poolItems = Array.Empty<PoolItem>();
            freeItems = new Queue<PoolItem>();
            AddItems(prefill);
        }
    }
}
