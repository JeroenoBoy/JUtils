using UnityEngine;

namespace JUtils
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Vector3 With(this Vector3 self, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? self.x, y ?? self.y, z ?? self.z);
        }


        /// <summary>
        /// add to a specific value of the vector
        /// </summary>
        public static Vector3 Add(this Vector3 self, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(self.x + x ?? 0, self.y + y ?? 0, self.z + z ?? 0);
        }


        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Vector3 ClampMagnitude(this Vector3 self, float maxForce)
        {
            return Vector3.ClampMagnitude(self, maxForce);
        }


        /// <summary>
        /// Get the closest vector from self
        /// </summary>
        /// <param name="a">The first target to check</param>">
        /// <param name="b">The second target to check</param>
        /// <returns>The closest vector</returns>
        public static Vector3 Closest(this Vector3 self, Vector3 a, Vector3 b)
        {
            float d1 = (a - self).sqrMagnitude;
            float d2 = (b - self).sqrMagnitude;
            return d1 < d2 ? a : b;
        }


        /// <summary>
        /// Round the Vector3
        /// </summary>
        /// <returns>The rounded position</returns>
        public static Vector2 XZToVector2(this Vector3 self)
        {
            return new Vector2(Mathf.Round(self.x),
                Mathf.Round(self.z)
            );
        }


        /// <summary>
        /// Round the Vector3
        /// </summary>
        /// <returns>The rounded position</returns>
        public static Vector3 Round(this Vector3 self)
        {
            return new Vector3(Mathf.Round(self.x),
                Mathf.Round(self.y),
                Mathf.Round(self.z)
            );
        }


        /// <summary>
        /// Floor the Vector3
        /// </summary>
        /// <returns>The rounded position</returns>
        public static Vector3 Floor(this Vector3 self)
        {
            return new Vector3(Mathf.Floor(self.x),
                Mathf.Floor(self.y),
                Mathf.Floor(self.z)
            );
        }


        /// <summary>
        /// Divide the Vector3 by a scalar
        /// </summary>
        public static Vector3 Divide(this Vector3 self, Vector3 other)
        {
            return new Vector3(self.x / other.x,
                self.y / other.y,
                self.z / other.z
            );
        }


        /// <summary>
        /// Multiply the Vector3 by a scalar
        /// </summary>
        public static Vector3 Multiply(this Vector3 self, Vector3 other)
        {
            return new Vector3(self.x * other.x,
                self.y * other.y,
                self.z * other.z
            );
        }


        /// <summary>
        /// Return the vector with absolute values
        /// </summary>
        /// <returns>The absolute position</returns>
        public static Vector3 Positive(this Vector3 self)
        {
            return new Vector3(Mathf.Abs(self.x),
                Mathf.Abs(self.y),
                Mathf.Abs(self.z)
            );
        }


        /// <summary>
        /// Return the vector with negative values
        /// </summary>
        /// <returns>The negative position</returns>
        public static Vector3 Negative(this Vector3 self)
        {
            return new Vector3(-Mathf.Abs(self.x),
                -Mathf.Abs(self.y),
                -Mathf.Abs(self.z)
            );
        }
    }
}