using System;
using System.Collections.Generic;
using JUtils.Extensions;
using UnityEngine;
using UnityEngine.Serialization;


namespace JUtils.FSM
{
    /// <summary>
    /// A mono-behaviour state-machine that can also be used as a state
    /// </summary>
    public abstract partial class StateMachine : State
    {
        [SerializeField] private bool _showLogs;
        [SerializeField] private bool _autoActivate;
        [SerializeField] private bool _autoCreateStates = true;

        public event Action<State> onStateChanged;

        protected bool              hasActiveState => currentState != null;
        protected State             currentState;
        protected Queue<QueueEntry> stateQueue = new ();

        
        protected virtual void Reset()
        {
            _autoActivate = !this.GetComponentInParentsDirect<StateMachine>();
        }


        protected virtual void OnValidate()
        {
            if (!_autoActivate || !this.GetComponentInParentsDirect<StateMachine>()) return;
            
            _autoActivate = false;
            Debug.LogWarning($"[{GetType().Name}] : {nameof(_autoActivate)} cannot be enabled when this is a sub-statemachine");
        }


        /// <summary>
        /// Clears the queue and triggers <see cref="OnNoState"/>>
        /// </summary>
        public void GoToNoState()
        {
            Log("Forcibly go to no state");
            stateQueue.Clear();
            ContinueQueue();
        }
        
        
        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState(State state, StateData data)
        {
            if (!state) {
                GoToNoState();
                return;
            }
            
            Log($"Force go to state '{state.GetType().Name}'");
            stateQueue.Clear();
            if (!AddToQueue(state, data)) ContinueQueue();
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T>(StateData data) where T : State
        {
            if (!TryFindState(out T state)) return;
            GoToState(state, data);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        /// <returns>True if the added state has been set as the current state</returns>
        public bool AddToQueue(State state, StateData data)
        {
            if (!state) {
                Log("Tried to add Null state");
            } else {
                stateQueue.Enqueue(new QueueEntry {state = state, data = data ?? new StateData()});
                Log($"Add '{state.GetType().Name}' to the queue");
            }
            
            if (currentState) return false;
            ContinueQueue();
            return true;
        }
        

        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T>(StateData data) where T : State
        {
            if (TryFindState(out T state)) AddToQueue(state, data);
        }
        
        
        /// <summary>
        /// Deactivate the current state and go to the next one
        /// </summary>
        public void ContinueQueue()
        {
            if (!isActive) {
                Debug.LogWarning($"[{GetType().Name}] Tried to continue the state queue, but the this state machine is not active!");
                return;
            }

            if (currentState) {
                Log($"Deactivate state '{currentState.GetType().Name}'");
                currentState.DeactivateState();
            }

            if (stateQueue.TryDequeue(out QueueEntry entry)) {
                State state = entry.state;

                state.stateMachine = this;
                currentState       = state;
                
                Log($"Activate state '{state.GetType().Name}'");
                state.ActivateState(entry.data);
                onStateChanged?.Invoke(state);
            }
            else {
                Log($"Firing function '{nameof(OnNoState)}'");
                currentState = null;
                OnNoState();
            }
        }
        

        /// <summary>
        /// Find a state within the child objects of this state-machine, if <see cref="_autoCreateStates"/> is enabled, it will automatically instantiate the state
        /// </summary>
        public T FindState<T>() where T : State
        {
            TryFindState(out T state);
            return state;
        }


        /// <summary>
        /// Try finding a state within the child objects of this state-machine, if <see cref="_autoCreateStates"/> is enabled, it will automatically instantiate that state
        /// </summary>
        public bool TryFindState<T>(out T state) where T : State
        {
            if (this.TryGetComponentInDirectChildren(out state)) 
                return true;

            if (!_autoCreateStates) {
                Debug.LogError($"[{GetType().Name}] : Tried to load state '{typeof(T).Name}' but it does not exist");
                return false;
            }
            
            Log($"Created new state '{typeof(T).Name}'");
            
            GameObject obj = new (typeof(T).Name);
            obj.transform.parent = transform;
            state = obj.AddComponent<T>();
            return true;
        }
        
        
        /// <summary>
        /// Internal function of activating the state
        /// </summary>
        internal override bool ActivateState(StateData data)
        {
            if (!base.ActivateState(data)) return false;
            if (!hasActiveState) {
                ContinueQueue();
            }
            return true;
        }


        /// <summary>
        /// Internal function for deactivating the state
        /// </summary>
        internal override void DeactivateState()
        {
            base.DeactivateState();
            stateQueue.Clear();
            
            if (!currentState) return;
            currentState.DeactivateState();
            currentState = null;
        }


        protected override void Awake()
        {
            if (!_autoActivate) base.Awake();
        }


        protected virtual void Start()
        {
            if (_autoActivate) { ActivateState(new StateData()); }
        }
        
        
        protected abstract void OnNoState();



        private void Log(object message)
        {
            if (_showLogs) {
                Debug.Log($"[{GetType().Name}] : {message}", this);
            }
        }


        
        /// <summary>
        /// Representation of the state and its data in the queue
        /// </summary>
        public struct QueueEntry
        {
            public State     state;
            public StateData data;
        }
    }
}
