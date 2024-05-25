using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Destroys the targetted gameobject when the attached HealthComponent died
    /// </summary>
    /// <remarks>If the target object has a <see cref="PoolItem"/> the object will call <see cref="PoolItem.ReturnToPool">ReturnToPool</see> instead</remarks>
    [RequireComponent(typeof(HealthComponent))]
    public sealed class DestroyObjectAfterKilled : MonoBehaviour
    {
        [SerializeField] private GameObject _targetObject;

        private void OnDeath()
        {
            if (_targetObject.TryGetComponent(out PoolItem poolItem)) {
                poolItem.ReturnToPool();
            } else {
                Destroy(_targetObject);
            }
        }
    }
}