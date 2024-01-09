using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    ///     A mono-behaviour state-machine that can also be used as a state
    /// </summary>
    public abstract partial class StateMachine : State
    {
        [SerializeField] private bool _showLogs;
        [SerializeField] private bool _autoActivate;
        [SerializeField] private bool _autoCreateStates = true;
        
        public event Action<State> onStateChanged;
        
        protected bool hasActiveState => currentState != null;
        protected bool isQueueFilled => stateQueue.Count > 0;
        protected bool isQueueEmpty => stateQueue.Count == 0;
        
        protected State currentState;
        protected List<QueueEntry> stateQueue = new();


        /// <summary>
        ///     Clears the queue and triggers <see cref="OnNoState" />>
        /// </summary>
        public void GoToNoState()
        {
            //  TODO: Make NoState Work
            //  TODO: Add QueueNoState
            Log("Forcibly go to no state");
            ContinueQueue();
        }


        /// <summary>
        ///     Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState([NotNull] State state, [CanBeNull] StateData data)
        {
            Log($"Go to state '{state.GetType().Name}'");
            AddToQueue(state, data, true);
            ContinueQueue();
        }


        /// <summary>
        ///     Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T>([CanBeNull] StateData data) where T : State
        {
            if (!TryFindState(out T state)) throw new Exception($"Could not find state '{typeof(T).Name}'");
            GoToState(state, data);
        }


        /// <summary>
        ///     Adds a new state to the queue
        /// </summary>
        public void AddToQueue([NotNull] State state, [CanBeNull] StateData data, bool queueFirst = false)
        {
            if (queueFirst) {
                stateQueue.Insert(0, new QueueEntry { state = state, data = data ?? new StateData() });
                Log($"Inserted '{state.GetType().Name}' to the queue");
            } else {
                stateQueue.Add(new QueueEntry { state = state, data = data ?? new StateData() });
                Log($"Add '{state.GetType().Name}' to the queue");
            }
        }


        /// <summary>
        ///     Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T>([CanBeNull] StateData data, bool queueFirst = false) where T : State
        {
            if (!TryFindState(out T state)) throw new Exception($"Could not find state '{typeof(T).Name}'");
            AddToQueue(state, data, queueFirst);
        }


        /// <summary>
        ///     Deactivate the current state and go to the next one
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

            QueueEntry nextEntry;
            if (isQueueFilled) {
                nextEntry = stateQueue[0];
                stateQueue.RemoveAt(0);
            } else {
                nextEntry = default;
            }
            
            if (nextEntry.state == null) {
                Log($"Running '{nameof(OnNoState)}'");
                currentState = null;
                OnNoState();
                return;
            }
            
            State state = nextEntry.state;

            state.stateMachine = this;
            currentState = state;

            Log($"Activate state '{state.GetType().Name}'");
            state.ActivateState(nextEntry.data);
            onStateChanged?.Invoke(state);
        }


        /// <summary>
        ///     Find a state within the child objects of this state-machine, if <see cref="_autoCreateStates" /> is enabled, it
        ///     will automatically instantiate the state
        /// </summary>
        public T FindState<T>() where T : State
        {
            TryFindState(out T state);
            return state;
        }


        /// <summary>
        ///     Try finding a state within the child objects of this state-machine, if <see cref="_autoCreateStates" /> is enabled,
        ///     it will automatically instantiate that state
        /// </summary>
        public bool TryFindState<T>(out T state) where T : State
        {
            if (this.TryGetComponentInDirectChildren(out state)) return true;
            if (!_autoCreateStates) return false;

            Log($"Created new state '{typeof(T).Name}'");

            GameObject obj = new(typeof(T).Name);
            state = obj.AddComponent<T>();
            obj.transform.parent = transform;
            return true;
        }


        /// <summary>
        ///     Internal function of activating the state
        /// </summary>
        internal override bool ActivateState([NotNull] StateData data)
        {
            if (!base.ActivateState(data)) return false;
            if (!hasActiveState) ContinueQueue();
            return true;
        }


        /// <summary>
        ///     Internal function for deactivating the state
        /// </summary>
        internal override void DeactivateState()
        {
            base.DeactivateState();
            stateQueue.Clear();

            if (!currentState) return;
            currentState.DeactivateState();
            currentState = null;
        }


        protected abstract void OnNoState();


        protected override void Awake()
        {
            if (!_autoActivate) base.Awake();
        }


        protected virtual void Start()
        {
            if (!_autoActivate) return;
            ActivateState(new StateData());
        }


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


        private void Log(object message)
        {
            if (!_showLogs) return;
            Debug.Log($"[{GetType().Name}] : {message}", this);
        }


        /// <summary>
        ///     Representation of the state and its data in the queue
        /// </summary>
        public struct QueueEntry
        {
            public State state;
            public StateData data;
        }
    }
}