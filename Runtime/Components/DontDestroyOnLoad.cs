using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Runs the DDOL function on this gameobject, this is a more declerative way to mark an object for DDOL
    /// </summary>
    public sealed class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}