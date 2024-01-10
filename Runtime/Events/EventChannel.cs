using System;
using UnityEngine;

namespace JUtils.Events
{
    /// <summary>
    /// The base class for an simple event channel
    /// 
    /// Event channels are scriptable objects that can dynamically be assigned
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ResourcePlayModeChangeCallbackReceiver("Events")]
    public abstract class EventChannel<T> : ScriptableObject
    {
        [SerializeField] private bool _clearEventsOnPlayModeExit = true;
        [SerializeField] private bool _checkForLeaksOnPlayModeExit = true;

        private event Action<T> listeners;


        /// <summary>
        /// Add a listener to this event channel
        /// </summary>
        public void AddListener(Action<T> listener)
        {
            listeners += listener;
        }


        /// <summary>
        /// Remove a listener from this event channel
        /// </summary>
        public void RemoveListener(Action<T> listener)
        {
            listeners -= listener;
        }


        /// <summary>
        /// Unsafe version of the <see cref="EventChannelExtensions.Raise"/> function. This is directly on the class, but it does not have check if the channel is null or not.
        /// </summary>
        /// <param name="argument"></param>
        public void RaiseUnsafe(T argument)
        {
            listeners?.Invoke(argument);
        }


#if UNITY_EDITOR
        public void OnPlayModeExit()
        {
            if (listeners == null) return;

            if (_checkForLeaksOnPlayModeExit) {
                int count = listeners.GetInvocationList().Length;
                if (count > 0) Debug.LogWarning($"<color=red>Event Leak Detected!</color> '{name}' had {count} listeners after exiting play mode!", this);
            }

            if (_clearEventsOnPlayModeExit) listeners = null;
        }
#endif
    }
}