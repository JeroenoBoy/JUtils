using UnityEngine;



namespace JUtils.Extensions
{
    public static class Vector2Extensions
    {
        
        /// <summary>
        /// Convert this Vector2 to Vector3 where X = X, Y = 0, Z = Y
        /// </summary>
        /// <returns>A vector3 where X = X, Y = 0, Z = Y</returns>
        public static Vector3 ToXZVector3(this Vector2 self)
        {
            return new Vector3(self.x, 0, self.y);
        }
    }
}
