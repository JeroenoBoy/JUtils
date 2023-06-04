using UnityEngine;



namespace JUtils.Components
{
    /// <summary>
    /// Copies this position to one of another object
    /// </summary>
    public class CopyPosition : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3   _offset;
        
        private void LateUpdate()
        {
            transform.position = _target.position + _offset;
        }
    }
}
