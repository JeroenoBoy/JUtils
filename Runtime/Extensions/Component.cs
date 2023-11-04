using System;
using System.Reflection;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Useful extensions for working with components
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Returns a Ray struct with the position and forward of this component's transform
        /// </summary>
        public static Ray ForwardRay(this Component self)
        {
            Transform transform = self.transform;
            return new Ray(transform.position, transform.forward);
        }


        /// <summary>
        /// Compare if a gameObject has a layer
        /// </summary>
        public static bool HasLayer(this Component comp, int layer)
        {
            GameObject gameObject = comp.gameObject;
            return ((1 << gameObject.layer) & layer) != 0 || layer == gameObject.layer;
        }


        /// <summary>
        /// Check if a component exists on the components game-object
        /// </summary>
        public static bool HasComponent<T>(this Component self) => self.GetComponent<T>() != null;


        /// <summary>
        /// Check if a component exists using the Type specification
        /// </summary>
        public static bool HasComponent(this Component self, Type type) => self.GetComponent(type) != null;


        /// <summary>
        /// Try get a component in the children
        /// </summary>
        public static bool TryGetComponentInChildren<T>(this Component self, out T component) where T : Component
        {
            component = self.GetComponentInChildren<T>();
            return component;
        }


        /// <summary>
        /// Get a component in the direct children of the component's transform. Won't look into children of children
        /// </summary>
        public static T GetComponentInDirectChildren<T>(this Component self) where T : Component
        {
            foreach (Transform t in self.transform) {
                if (t.TryGetComponent(out T component))
                    return component;
            }
            return null;
        }
        
        
        /// <summary>
        /// Try get a component in in the direct children of this component
        /// </summary>
        public static bool TryGetComponentInDirectChildren<T>(this Component self, out T component) where T : Component
        {
            component = self.GetComponentInDirectChildren<T>();
            return component;
        }


        /// <summary>
        /// Get a component in the direct parents of this component (Without this my own component)
        /// </summary>
        public static T GetComponentInParentsDirect<T>(this Component self) where T : Component
        {
            return self.transform.parent ? self.transform.parent.GetComponentInParent<T>() : null;
        }


        /// <summary>
        /// Copy one component to another
        /// Source: https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
        /// </summary>
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (!pinfo.CanWrite) continue;
                try {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos) {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }
    }
}
