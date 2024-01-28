using System;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A struct useful for showing in the inspector if the value is optional, and does not have to be set. Also allows for quicker checks if the value is set
    /// </summary>
    /// <example><code>
    /// namespace Example
    /// {
    ///     public class OptionalExample : MonoBehaviour
    ///     {
    ///         [SerializeField] private Optional&#60;HealthComponent> _healthComponent;
    ///
    ///         private void Start()
    ///         {
    ///             HealthComponent hc = _healthComponent ? _healthComponent : GetComponent&#60;HealthComponent>();
    ///             hc.Damage(10);
    ///         }
    ///     }
    /// }
    /// </code></example>
    [Serializable]
    public struct Optional<T>
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private T _value;

        public bool enabled => _enabled && _value is not null;
        public T value => enabled ? _value : throw new NullReferenceException();


        public Optional(T initialValue)
        {
            _enabled = initialValue is not null;
            _value = initialValue;
        }


        public static implicit operator bool(Optional<T> optional)
        {
            return optional.enabled;
        }

        public static implicit operator T(Optional<T> optional)
        {
            return optional._value;
        }
    }
}