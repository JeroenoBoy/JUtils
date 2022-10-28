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
    }
}
