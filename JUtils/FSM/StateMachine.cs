using System;
using System.Collections.Generic;
using JUtils.Extensions;
using UnityEngine;



namespace JUtils.FSM
{
    public abstract partial class StateMachine : State
    {
        [SerializeField] private bool showLogs;
        [SerializeField] private bool autoActivate;
        [SerializeField] private bool autoCreateStates = true;

        protected State             CurrentState;
        protected Queue<QueueEntry> StateQueue = new ();


        protected virtual void Reset()
        {
            autoActivate = !GetComponentInParent<StateMachine>();
        }


        protected virtual void OnValidate()
        {
            if (!autoActivate || !this.GetComponentInParentsDirect<StateMachine>()) return;
            
            autoActivate = false;
            Debug.LogWarning($"[{GetType().Name}] : {nameof(autoActivate)} cannot be enabled when this is a sub-statemachine");
        }


        /// <summary>
        /// Clears the queue and triggers <see cref="OnNoState"/>>
        /// </summary>
        public void GoToNoState()
        {
            Log("Forcibly go to no state");
            StateQueue.Clear();
            ContinueQueue();
        }
        
        
        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState(State state, params object[] arguments)
        {
            if (!state) {
                GoToNoState();
                return;
            }
            
            Log($"Force go to state '{state.GetType().Name}'");
            StateQueue.Clear();
            if (!AddToQueue(state, arguments)) ContinueQueue();
        }


        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T>(params object[] arguments) where T : State
        {
            if (!TryFindState(out T state)) return;
            GoToState(state, arguments);
        }


        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        /// <returns>True if the added state has been set as the current state</returns>
        public bool AddToQueue(State state, params object[] arguments)
        {
            if (!state) {
                Log($"Tried to add Null state");
            }
            else {
                StateQueue.Enqueue(new QueueEntry {State = state, Data = new StateData(arguments)});
                Log($"Add '{state.GetType().Name}' to the queue");
            }
            
            if (CurrentState) return false;
            ContinueQueue();
            return true;
        }
        

        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T>(params object[] arguments) where T : State
        {
            if (TryFindState(out T state)) AddToQueue(state, arguments);
        }
        
        
        /// <summary>
        /// Deactivate the current state and go to the next one
        /// </summary>
        public void ContinueQueue()
        {
            if (!IsActive) {
                Debug.LogWarning($"[{GetType().Name}] Tried to continue the state queue, but the this state machine is not active!");
                return;
            }

            if (CurrentState) {
                Log($"Deactivate state '{CurrentState.GetType().Name}'");
                CurrentState.DeactivateState();
            }

            if (StateQueue.TryDequeue(out QueueEntry entry)) {
                State state = entry.State;

                state.Data         = entry.Data;
                state.StateMachine = this;
                CurrentState       = state;
                
                Log($"Activate state '{state.GetType().Name}'");
                state.ActivateState();
            }
            else {
                Log($"Firing function '{nameof(OnNoState)}'");
                CurrentState = null;
                OnNoState();
            }
        }


        protected override void OnActivate()
        {
            ContinueQueue();
        }


        protected override void OnDeactivate()
        {
            StateQueue.Clear();

            if (!CurrentState) return;
            CurrentState.DeactivateState();
            CurrentState = null;
        }


        protected override void Awake()
        {
            if (!autoActivate) base.Awake();
        }


        protected virtual void Start()
        {
            if (autoActivate) { ActivateState(); }
        }
        
        
        protected abstract void OnNoState();


        private bool TryFindState<T>(out T state) where T : State
        {
            if (this.TryGetComponentInDirectChildren(out state)) 
                return true;

            if (!autoCreateStates) {
                Debug.LogError($"[{GetType().Name}] : Tried to load state '{typeof(T).Name}' but it does not exist");
                return false;
            }
            
            Log($"Created new state '{typeof(T).Name}'");
            
            GameObject obj = new (typeof(T).Name);
            obj.transform.parent = transform;
            state = obj.AddComponent<T>();
            return true;
        }


        private void Log(object message)
        {
            if (showLogs) {
                Debug.Log($"[{GetType().Name}] : {message}", this);
            }
        }



        public struct QueueEntry
        {
            public State     State;
            public StateData Data;
        }
    }
}
