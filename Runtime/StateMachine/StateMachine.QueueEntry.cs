namespace JUtils
{
    public abstract partial class StateMachine
    {
        /// <summary>
        /// Representation of the state and its data in the queue
        /// </summary>
        public struct QueueEntry
        {
            public State state;
            public StateData data;
        }
    }
}