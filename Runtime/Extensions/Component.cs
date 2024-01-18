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
        public static bool HasComponent<T>(this Component self)
        {
            return self.GetComponent<T>() != null;
        }


        /// <summary>
        /// Check if a component exists using the Type specification
        /// </summary>
        public static bool HasComponent(this Component self, Type type)
        {
            return self.GetComponent(type) != null;
        }


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
                if (t.TryGetComponent(out T component)) return component;
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
        public static bool CopyTo<T>(this T self, T target) where T : Component
        {
            Type type = self.GetType();
            if (type != target.GetType()) return false;

            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

            foreach (PropertyInfo propertyInfo in type.GetProperties(flags)) {
                if (!propertyInfo.CanWrite) continue;
                try {
                    propertyInfo.SetValue(target, propertyInfo.GetValue(self, null), null);
                } catch {
                    // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }

            foreach (FieldInfo fieldInfo in type.GetFields(flags)) {
                fieldInfo.SetValue(target, fieldInfo.GetValue(self));
            }

            return true;
        }
    }
}