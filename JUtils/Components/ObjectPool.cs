using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



namespace JUtils.Components
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private PoolItem prefab;
        [SerializeField] private int prefill = 10;
        [SerializeField] private int maxSize = -1;
        
        [SerializeField] private bool autoExpand = true;
        [SerializeField] private int autoExpandAmount = 5;
        
        private PoolItem[] poolItems;
        private Queue<PoolItem> freeItems;


        public PoolItem GetItem()
        {
            TryGetItem(out PoolItem item);
            return item;
        }
        
        
        public bool TryGetItem(out PoolItem item)
        {
            if (freeItems.Count == 0 && autoExpand && AddItems() == 0) {
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
                PoolItem item = Instantiate(prefab);
                item.ObjectPool = this;
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
            item.transform.parent = transform;
            freeItems.Enqueue(item);
            
            return true;
        }


        private void Start()
        {
            poolItems = Array.Empty<PoolItem>();
            freeItems = new Queue<PoolItem>();
            AddItems(prefill);
        }
    }
}
