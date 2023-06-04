using UnityEngine;



namespace JUtils.Components
{
    /// <summary>
    /// Runs the DDOL function on this gameobject, this is a more declerative way to mark an object for DDOL
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
