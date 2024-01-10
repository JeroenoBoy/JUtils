using System;
using UnityEngine;
using UnityEngine.Events;

namespace JUtils.Events
{
    /// <summary>
    /// The base class for event channel based listeners. Recommended for UI or if the GO only has one event listener, otherwise consider using the <see cref="EventChannel{T}"/> directly
    /// </summary>
    public abstract class EventListener<TListener, TArgument> : MonoBehaviour where TListener : EventChannel<TArgument>
    {
        [SerializeField] private TListener _eventChannel;
        [SerializeField] private UnityEvent<TArgument> _event;

        private event Action<TArgument> myListeners;


        /// <summary>
        /// Add a listener to the internal eventListener of this class
        /// </summary>
        public void AddListener(Action<TArgument> listener)
        {
            myListeners += listener;
        }


        /// <summary>
        /// Remove a listener to the internal eventListener of this class
        /// </summary>
        public void RemoveListener(Action<TArgument> listener)
        {
            myListeners -= listener;
        }


        private void OnEnable()
        {
            _eventChannel.AddListener(HandleChannelInvoked);
        }


        private void OnDisable()
        {
            _eventChannel.RemoveListener(HandleChannelInvoked);
        }


        private void HandleChannelInvoked(TArgument argument)
        {
            myListeners?.Invoke(argument);
            _event.Invoke(argument);
        }
    }
}