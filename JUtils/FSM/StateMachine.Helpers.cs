namespace JUtils.FSM
{
    public abstract partial class StateMachine
    {
        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState(State state)
        {
            AddToQueue(state, new StateData());
        }
        
        
        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1>(T state, T1 arg1) where T : State<T1>
        {
            GoToState(state, new StateData(arg1));
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1, T2>(T state, T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            GoToState(state, new StateData(arg1,arg2));
        }
        

        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1, T2, T3>(T state, T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            GoToState(state, new StateData(arg1, arg2, arg3));
        }
        
        
        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void GoToState<T>() where T : State
        {
            GoToState<T>(new StateData());
        }
        
        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1>(T1 arg1) where T : State<T1>
        {
            GoToState<T>(new StateData(arg1));
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1, T2>(T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            GoToState<T>(new StateData(arg1, arg2));
        }
        

        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            GoToState<T>(new StateData(arg1, arg2, arg3));
        }
        
        
        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T>(T state) where T : State
        {
            AddToQueue(state, new StateData());
        }

        
        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1>(T state, T1 arg1) where T : State<T1>
        {
            AddToQueue(state, new StateData(arg1));
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1, T2>(T state, T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            AddToQueue(state, new StateData(arg1, arg2));
        }
        

        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1, T2, T3>(T state, T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            AddToQueue(state, new StateData(arg1, arg2, arg3));
        }
       
        
        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T>() where T : State
        {
            AddToQueue<T>(new StateData());
        }

        
        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1>(T1 arg1) where T : State<T1>
        {
            AddToQueue<T>(new StateData(arg1));
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1, T2>(T1 arg1, T2 arg2) where T : State<T1, T2>
        {
            AddToQueue<T>(new StateData(arg1, arg2));
        }
        

        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3) where T : State<T1, T2, T3>
        {
            AddToQueue<T>(new StateData(arg1, arg2, arg3));
        }
    }
}
