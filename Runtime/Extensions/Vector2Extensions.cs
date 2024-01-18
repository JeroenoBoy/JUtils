using UnityEngine;

namespace JUtils
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


        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Vector2 With(this Vector2 self, float? x = null, float? y = null)
        {
            return new Vector2(x ?? self.x, y ?? self.y);
        }


        /// <summary>
        /// add to a specific value of the vector
        /// </summary>
        public static Vector2 Add(this Vector2 self, float? x = null, float? y = null)
        {
            return new Vector2(self.x + x ?? 0, self.y + y ?? 0);
        }


        /// <summary>
        /// Clamp the magnitude of the vector
        /// </summary>
        public static Vector2 ClampMagnitude(this Vector2 self, float maxForce)
        {
            return Vector2.ClampMagnitude(self, maxForce);
        }


        /// <summary>
        /// Get the closest vector from self
        /// </summary>
        /// <param name="a">The first target to check</param>">
        /// <param name="b">The second target to check</param>
        /// <returns>The closest vector</returns>
        public static Vector2 Closest(this Vector2 self, Vector2 a, Vector2 b)
        {
            float d1 = (a - self).sqrMagnitude;
            float d2 = (b - self).sqrMagnitude;
            return d1 < d2 ? a : b;
        }


        /// <summary>
        /// Round the Vector3
        /// </summary>
        /// <returns>The rounded position</returns>
        public static Vector2 Round(this Vector2 self)
        {
            return new Vector2(Mathf.Round(self.x),
                Mathf.Round(self.y)
            );
        }


        /// <summary>
        /// Floor the Vector3
        /// </summary>
        /// <returns>The rounded position</returns>
        public static Vector2 Floor(this Vector2 self)
        {
            return new Vector2(Mathf.Floor(self.x),
                Mathf.Floor(self.y)
            );
        }


        /// <summary>
        /// Multiply the Vector3 by a scalar
        /// </summary>
        public static Vector2 Multiply(this Vector2 self, Vector2 other)
        {
            return new Vector2(self.x * other.x,
                self.y * other.y
            );
        }


        /// <summary>
        /// Return the vector with absolute values
        /// </summary>
        /// <returns>The absolute position</returns>
        public static Vector2 Positive(this Vector2 self)
        {
            return new Vector2(Mathf.Abs(self.x),
                Mathf.Abs(self.y)
            );
        }


        /// <summary>
        /// Return the vector with negative values
        /// </summary>
        /// <returns>The negative position</returns>
        public static Vector2 Negative(this Vector2 self)
        {
            return new Vector2(-Mathf.Abs(self.x),
                -Mathf.Abs(self.y)
            );
        }
    }
}