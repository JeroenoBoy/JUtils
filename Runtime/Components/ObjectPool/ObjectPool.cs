using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A simple implementation for an object pool
    /// </summary>
    /// <remarks>Disabling this component may result in erroneous behaviour</remarks>
    public sealed class ObjectPool : MonoBehaviour
    {
        [SerializeField] private PoolItem _template;
        [SerializeField] private int _maxSize = -1;

        [SerializeField] private bool _autoExpand = true;
        [SerializeField] private int _autoExpandAmount = 5;

        public PoolItem template => _template;
        public IEnumerable<PoolItem> activePoolItems => _poolItems;
        public IEnumerable<PoolItem> freePoolItems => _freeItems;

        private PoolItem[] _poolItems;
        private List<PoolItem> _freeItems;

        private bool _templateChangeable = true;
        private bool _isGettingDestroyed = true;

        /// <summary>
        /// Spawn a pooled item
        /// </summary>
        public PoolItem SpawnItem()
        {
            return SpawnItem(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Spawn a pooled item
        /// </summary>
        public PoolItem SpawnItem(Vector3 localPosition)
        {
            return SpawnItem(localPosition, Quaternion.identity);
        }

        /// <summary>
        /// Spawn a pooled item
        /// </summary>
        public PoolItem SpawnItem(Vector3 localPosition, Transform parent)
        {
            return SpawnItem(localPosition, Quaternion.identity, parent);
        }

        /// <summary>
        /// Spawn a pooled item
        /// </summary>
        public PoolItem SpawnItem(Transform parent)
        {
            return SpawnItem(Vector3.zero, Quaternion.identity, parent);
        }

        /// <summary>
        /// Spawn a pooled item
        /// </summary>
        public PoolItem SpawnItem(Vector3 localPosition, Quaternion localRotation, Transform parent = null)
        {
            if (!TryGetInternal(out PoolItem item)) return null;
            Transform itemTransform = item.transform;

            itemTransform.parent = parent;
            itemTransform.localPosition = localPosition;
            itemTransform.localRotation = localRotation;

            item.Spawn();

            return item;
        }

        /// <summary>
        /// Get a pooled item, returns false if the pool can't auto expand and if there are no items left
        /// </summary>
        public bool TryGetItem(out PoolItem item)
        {
            if (!TryGetInternal(out item)) return false;
            item.Spawn();
            return true;
        }

        /// <summary>
        /// Request X amount of items to be made
        /// </summary>
        public int InstantiateNewItems(int amount = -1)
        {
            _templateChangeable = false;
            if (amount <= 0) amount = _autoExpandAmount;
            int newSize = _maxSize > 0 ? Mathf.Max(_poolItems.Length, amount + _poolItems.Length) : amount + _poolItems.Length;
            amount = newSize - _poolItems.Length;

            if (newSize == _poolItems.Length) return 0;

            Array.Resize(ref _poolItems, newSize);

            while (amount-- > 0) {
                InstantiatePoolItem(newSize - amount - 1);
            }

            return amount;
        }

        /// <summary>
        /// Change the template of this object pool
        /// </summary>
        /// <remarks>Template can only change when no object has been created</remarks>
        public void SetTemplate(PoolItem newTemplate)
        {
            if (!_templateChangeable) {
                throw new Exception($"Template of object pool {name} is no longer changeable");
            }

            _template = newTemplate;
        }

        internal bool ReturnItem(PoolItem item)
        {
            if (!_poolItems.Contains(item) || _freeItems.Contains(item)) return false;

            item.Despawn();
            Transform itemTransform = item.transform;
            itemTransform.parent = transform;
            itemTransform.localPosition = Vector3.zero;
            itemTransform.localRotation = Quaternion.identity;

            _freeItems.Add(item);
            return true;
        }

        internal void PoolItemDestroyed(PoolItem destroyedElement)
        {
            if (this == null || _isGettingDestroyed) return;
            int index = _poolItems.IndexOf(destroyedElement);
            InstantiatePoolItem(index);
        }

        private bool TryGetInternal(out PoolItem item)
        {
            if (_freeItems.Count == 0 && (!_autoExpand || InstantiateNewItems() == 0)) {
                item = null;
                return false;
            }

            item = _freeItems[^1];
            _freeItems.Remove(item);

            return true;
        }

        private void Awake()
        {
            _freeItems = new List<PoolItem>();
            _poolItems = Array.Empty<PoolItem>();
        }

        private void OnEnable()
        {
            _isGettingDestroyed = false;
        }

        private void OnDisable()
        {
            _isGettingDestroyed = true;
        }

        private void InstantiatePoolItem(int poolIndex)
        {
            PoolItem item = Instantiate(_template.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<PoolItem>();
            item.objectPool = this;
            item.gameObject.SetActive(false);
            _poolItems[poolIndex] = item;
            _freeItems.Add(item);
        }
    }
}