using System.Runtime.CompilerServices;

namespace JUtils
{
    public abstract partial class StateMachine
    {
        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState(State state)
        {
            GoToState(state, new StateData());
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1>(T state, T1 arg1) where T : State<T1>
        {
            GoToState(state, new StateData(arg1));
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1, T2>(T state, T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            GoToState(state, new StateData(arg1, arg2));
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1, T2, T3>(T state, T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            GoToState(state, new StateData(arg1, arg2, arg3));
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T>() where T : State
        {
            GoToState<T>(new StateData());
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1>(T1 arg1) where T : State<T1>
        {
            GoToState<T>(new StateData(arg1));
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1, T2>(T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            GoToState<T>(new StateData(arg1, arg2));
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            GoToState<T>(new StateData(arg1, arg2, arg3));
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1, T2, T3>(StateRef<T> stateRef, T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            GoToState<T>(new StateData(arg1, arg2, arg3));
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1, T2>(StateRef<T> stateRef, T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            GoToState<T>(new StateData(arg1, arg2));
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T, T1>(StateRef<T> stateRef, T1 arg1) where T : State<T1>
        {
            GoToState<T>(new StateData(arg1));
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GoToState<T>(StateRef<T> stateRef) where T : State
        {
            GoToState<T>();
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T>(T state, bool queueFist = false) where T : State
        {
            AddToQueue(state, new StateData(), queueFist);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1>(T state, T1 arg1, bool queueFist = false) where T : State<T1>
        {
            AddToQueue(state, new StateData(arg1), queueFist);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1, T2>(T state, T1 arg1, T2 arg2, bool queueFist = false) where T : State<T1, T2>
        {
            AddToQueue(state, new StateData(arg1, arg2), queueFist);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1, T2, T3>(T state, T1 arg1, T2 arg2, T3 arg3, bool queueFirst = false) where T : State<T1, T2, T3>
        {
            AddToQueue(state, new StateData(arg1, arg2, arg3), queueFirst);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T>(bool queueFist = false) where T : State
        {
            AddToQueue<T>(new StateData(), queueFist);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1>(T1 arg1, bool queueFirst = false) where T : State<T1>
        {
            AddToQueue<T>(new StateData(arg1), queueFirst);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1, T2>(T1 arg1, T2 arg2, bool queueFirst = false) where T : State<T1, T2>
        {
            AddToQueue<T>(new StateData(arg1, arg2), queueFirst);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3, bool queueFirst = false) where T : State<T1, T2, T3>
        {
            AddToQueue<T>(new StateData(arg1, arg2, arg3), queueFirst);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1, T2, T3>(StateRef<T> stateRef, T1 arg1, T2 arg2, T3 arg3, bool queueFirst = false) where T : State<T1, T2, T3>
        {
            AddToQueue<T>(new StateData(arg1, arg2, arg3), queueFirst);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1, T2>(StateRef<T> stateRef, T1 arg1, T2 arg2, bool queueFirst = false) where T : State<T1, T2>
        {
            AddToQueue<T>(new StateData(arg1, arg2), queueFirst);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T, T1>(StateRef<T> stateRef, T1 arg1, bool queueFirst = false) where T : State<T1>
        {
            AddToQueue<T>(new StateData(arg1), queueFirst);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddToQueue<T>(StateRef<T> stateRef, bool queueFirst = false) where T : State
        {
            AddToQueue<T>(queueFirst);
        }
    }
}