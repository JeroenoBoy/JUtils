﻿using JetBrains.Annotations;
using UnityEngine;

namespace JUtils
{
    public static class UnityObject
    {
        /// <summary>
        /// Will return null if the object is null, or if it has been destroyed
        /// </summary>
        [CanBeNull]
        public static Object OrNull(this Object obj)
        {
            return obj == null ? null : obj;
        }
    }
}