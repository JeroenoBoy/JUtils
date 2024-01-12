using System.Collections.Generic;
using UnityEngine;

namespace JUtils.Internal
{
    /// <summary>
    /// The class that keeps all references for StaticListBehaviour`1
    /// </summary>
    internal sealed class MultitonBehaviourManager : MonoBehaviour
    {
        internal static List<T> GetList<T>() where T : Object
        {
            MultitonBehaviourManager manager = JUtilsObject.instance.multitonBehaviourManager;

            if (manager._items.TryGetValue(SerializableType.Get<T>(), out List<Object> objects)) return objects as List<T>;

            List<T> newList = new();
            manager._items.Add(SerializableType.Get<T>(), newList as List<Object>);
            return newList;
        }


        internal static bool Remove<T>() where T : Object
        {
            MultitonBehaviourManager manager = JUtilsObject.instance.multitonBehaviourManager;
            return manager._items.Remove(new SerializableType(typeof(List<T>)));
        }


        [SerializeField] private SerializableDictionary<SerializableType, List<Object>> _items;


        private void Awake()
        {
            _items = new SerializableDictionary<SerializableType, List<Object>>();
        }
    }
}