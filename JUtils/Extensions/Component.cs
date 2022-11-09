using System.Reflection;
using System;
using UnityEngine;



namespace JUtils.Extensions
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// Set a specific value of the vector
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
        /// Copy one component to another
        /// Source: https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="other"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos) {
                if (pinfo.CanWrite) {
                    try {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos) {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }
    }
}
