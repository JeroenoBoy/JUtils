using System;
using JetBrains.Annotations;
using UnityEngine;

namespace JUtils.Events
{
    /// <summary>
    /// The base class for an simple event channel
    /// 
    /// Event channels are scriptable objects that can dynamically be assigned
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventChannel<T> : ScriptableObject
    {
        private event Action<T> channel;


        /// <summary>
        /// Add a listener to this event channel
        /// </summary>
        public void Register(Action<T> listener)
        {
            channel += listener;
        }


        /// <summary>
        /// Remove a listener from this event channel
        /// </summary>
        public void UnRegister(Action<T> listener)
        {
            channel -= listener;
        }


        /// <summary>
        /// Unsafe version of the <see cref="EventChannelExtensions.Raise"/> function. This is directly on the class, but it does not have check if the channel is null or not.
        /// </summary>
        /// <param name="argument"></param>
        public void RaiseUnsafe(T argument)
        {
            channel?.Invoke(argument);
        }
    }


    public static class EventChannelExtensions
    {
        public static void Raise<T>([CanBeNull] this EventChannel<T> eventChannel, T argument)
        {
            if (eventChannel == null) return;
            eventChannel.RaiseUnsafe(argument);
        }

        public static void Raise([CanBeNull] this EmptyEventChannel eventChannel)
        {
            if (eventChannel == null) return;
            eventChannel.RaiseUnsafe(default);
        }
    }
}