using JetBrains.Annotations;

namespace JUtils.Events
{
    /// <summary>
    /// Useful extensions for dealing with <see cref="EventChannel{T}"/>>
    /// </summary>
    public static class EventChannelExtensions
    {
        /// <summary>
        /// Raise an event on an <see cref="EventChannel{T}"/>.
        /// </summary>
        /// <remarks>This automatically checks if the channel is null</remarks>
        public static void Raise<T>([CanBeNull] this EventChannel<T> eventChannel, T argument)
        {
            if (eventChannel == null) return;
            eventChannel.RaiseUnsafe(argument);
        }

        /// <summary>
        /// Raise an event on an <see cref="EmptyEventChannel"/>>.
        /// </summary>
        /// <remarks>This automatically checks if the channel is null</remarks>
        public static void Raise([CanBeNull] this EmptyEventChannel eventChannel)
        {
            if (eventChannel == null) return;
            eventChannel.RaiseUnsafe(default);
        }
    }
}