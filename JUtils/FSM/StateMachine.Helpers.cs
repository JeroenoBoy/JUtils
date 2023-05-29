namespace JUtils.FSM
{
    public abstract partial class StateMachine
    {
        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1>(T state, T1 arg1) where T : State<T1>
        {
            GoToState(state, (object)arg1);
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1, T2>(T state, T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            GoToState(state, (object)arg1, (object)arg2);
        }
        

        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1, T2, T3>(T state, T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            GoToState(state, (object)arg1, arg2, arg3);
        }
       
        
        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1>(T1 arg1) where T : State<T1>
        {
            GoToState<T>((object)arg1);
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1, T2>(T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            GoToState<T>((object)arg1, (object)arg2);
        }
        

        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            GoToState<T>((object)arg1, arg2, arg3);
        }
        
        
        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1>(T state, T1 arg1) where T : State<T1>
        {
            AddToQueue(state, (object)arg1);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1, T2>(T state, T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            AddToQueue(state, (object)arg1, (object)arg2);
        }
        

        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1, T2, T3>(T state, T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            AddToQueue(state, (object)arg1, arg2, arg3);
        }
       
        
        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1>(T1 arg1) where T : State<T1>
        {
            AddToQueue<T>((object)arg1);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1, T2>(T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            AddToQueue<T>((object)arg1, (object)arg2);
        }
        

        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            AddToQueue<T>((object)arg1, arg2, arg3);
        }
    }
}
