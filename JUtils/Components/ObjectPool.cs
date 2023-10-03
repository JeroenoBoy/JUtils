using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;



namespace JUtils.Components
{
    /// <summary>
    /// A simple implementation for an object pool
    /// </summary>
    public sealed class ObjectPool : MonoBehaviour
    {
        [SerializeField] private PoolItem _template;
        [SerializeField] private int _prefill = 10;
        [SerializeField] private int _maxSize = -1;
        
        [SerializeField] private bool _autoExpand = true;
        [SerializeField] private int _autoExpandAmount = 5;
        
        private PoolItem[] _poolItems;
        private List<PoolItem> _freeItems;


        public PoolItem SpawnItem() => SpawnItem(Vector3.zero, Quaternion.identity);
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
            if (_freeItems.Count == 0 && (!_autoExpand || InstantiateNewItems() <= 0)) {
                Debug.LogWarning("Tried to get item from empty pool");
                item = null;
                return false;
            }

            item = _freeItems[^1];
            _freeItems.Remove(item);
            item.Spawn();

            return true;
        }
        
        
        public int InstantiateNewItems(int amount = -1)
        {
            if (amount <= 0) amount = _autoExpandAmount;
            int newSize = _maxSize > 0 ? Mathf.Max(_poolItems.Length, amount + _poolItems.Length) : amount + _poolItems.Length;
            amount = newSize - _poolItems.Length;

            if (newSize == _poolItems.Length) {
                return 0;
            }
            
            Array.Resize(ref _poolItems, newSize);

            while (amount --> 0) {
                PoolItem item = Instantiate(_template, Vector3.zero, Quaternion.identity, transform);
                item.objectPool = this;
                item.gameObject.SetActive(false);
                _poolItems[newSize - amount - 1] = item;
                _freeItems.Add(item);
            }
            
            return amount;
        }


        internal bool ReturnItem(PoolItem item)
        {
            if (!_poolItems.Contains(item) || _freeItems.Contains(item)) {
                return false;
            }

            item.Despawn();
            Transform transform = item.transform;
            
            transform.parent        = this.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            
            _freeItems.Add(item);
            return true;
        }


        private void Awake()
        {
            if (_template == null) {
                Debug.LogError("No template has been set!", this);
                return;
            }

            if (_template.transform.parent != transform) return;
            
            _template.gameObject.SetActive(false);
            _template.objectPool = this;
        }
        

        private void Start()
        {
            if (!_template) return;
            
            _poolItems = Array.Empty<PoolItem>();
            _freeItems = new List<PoolItem>();
            InstantiateNewItems(_prefill);
        }
    }
}