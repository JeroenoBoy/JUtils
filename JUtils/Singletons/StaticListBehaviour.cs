﻿using System.Collections.Generic;
using JUtils.Internal;
using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// A MonoBehaviour where you can easily access all of its instances
    /// </summary>
    public abstract class StaticListBehaviour<T> : MonoBehaviour where T : StaticListBehaviour<T>
    {
        private static List<T> _list;
        
        /// <summary>
        /// Get all enables instances of this type
        /// </summary>
        public static List<T> all => _list ??= StaticListBehaviourManager.GetList<T>();
        
        
        protected virtual void OnEnable()
        {
            all.Add(this as T);
        }


        protected virtual void OnDisable()
        {
            all.Remove(this as T);
        }
    }
}
