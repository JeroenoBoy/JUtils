using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A mono behaviour state-machine that can also be used as a state
    /// </summary>
    public abstract partial class StateMachine : State
    {
        [SerializeField] private bool _showLogs;
        [SerializeField] private bool _autoActivate;
        [SerializeField] private bool _autoCreateStates = true;

        public event Action<State> onStateChanged;

        public bool hasActiveState => currentState != null;
        public bool isQueueFilled => stateQueue.Count > 0;
        public bool isQueueEmpty => stateQueue.Count == 0;

        [CanBeNull]
        public new StateMachine stateMachine => base.stateMachine; // Added this to hopefully help with accidental calls to sub state machines that do not exist

        protected State currentState;
        protected List<QueueEntry> stateQueue = new();

        /// <summary>
        /// Queues the current given state, then goes to it
        /// </summary>
        public void GoToState([NotNull] State state, [CanBeNull] StateData data)
        {
            Log($"Go to state '{state.GetType().Name}'");
            AddToQueueInternal(state, data, true);
            ContinueQueue();
        }

        /// <summary>
        /// Clears the StateQueue and goes to the given state
        /// </summary>
        public void GoToState<T>([CanBeNull] StateData data) where T : State
        {
            if (!TryFindState(out T state)) throw new Exception($"Could not find state '{typeof(T).Name}'");
            GoToState(state, data);
        }

        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue([NotNull] State state, [CanBeNull] StateData data, bool queueFirst = false)
        {
            AddToQueueInternal(state, data, queueFirst);
            Log(queueFirst ? $"Inserted '{state.GetType().Name}' to the queue" : $"Added '{state.GetType().Name}' to the queue");
        }

        /// <summary>
        /// Adds a new state to the queue
        /// </summary>
        public void AddToQueue<T>([CanBeNull] StateData data, bool queueFirst = false) where T : State
        {
            if (!TryFindState(out T state)) throw new Exception($"Could not find state '{typeof(T).Name}'");
            AddToQueue(state, data, queueFirst);
        }

        /// <summary>
        /// Clears all states in the queue
        /// </summary>
        public void ClearQueue()
        {
            stateQueue.Clear();
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
        /// Find a state within the child objects of this state-machine, if <see cref="_autoCreateStates" /> is enabled, it
        /// will automatically instantiate the state
        /// </summary>
        public T FindState<T>() where T : State
        {
            TryFindState(out T state);
            return state;
        }

        /// <summary>
        /// Try finding a state within the child objects of this state-machine, if <see cref="_autoCreateStates" /> is enabled,
        /// it will automatically instantiate that state
        /// </summary>
        public bool TryFindState<T>(out T state) where T : State
        {
            if (this.TryGetComponentInDirectChildren(out state)) return true;
            if (!_autoCreateStates) return false;

            Log($"Created new state '{typeof(T).Name}'");

            GameObject obj = new(typeof(T).Name);
            obj.transform.parent = transform;
            state = obj.AddComponent<T>();
            return true;
        }

        /// <summary>
        /// Activates the state machine
        /// </summary>
        public void Activate([CanBeNull] StateData data = null)
        {
            if (isActive) return;
            ActivateState(data ?? new StateData());
        }

        /// <summary>
        /// Continues the queue in the state machine, or deactivates the state if it is the root state machine
        /// </summary>
        public new void Deactivate()
        {
            if (!isActive) return;
            if (stateMachine == null) {
                DeactivateState();
            } else {
                base.Deactivate();
            }
        }

        /// <summary>
        /// Internal function for deactivating the state
        /// </summary>
        internal override void DeactivateState()
        {
            base.DeactivateState();
            if (currentState) {
                Debug.LogWarning($"[{GetType().Name}] : State machine has been deactivated, but state '{currentState.GetType().Name}' is still active!");
            }
        }

        /// <summary>
        /// Gets called when there is no next state in the queue
        /// </summary>
        protected abstract void OnNoState();

        protected override void Awake()
        {
            if (!_autoActivate) base.Awake();
        }

        protected virtual void Start()
        {
            if (!_autoActivate) return;
            if (isActive) return;
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

        private void AddToQueueInternal(State state, StateData data, bool queueFirst)
        {
            if (queueFirst)
                stateQueue.Insert(0, new QueueEntry { state = state, data = data ?? new StateData() });
            else
                stateQueue.Add(new QueueEntry { state = state, data = data ?? new StateData() });
        }

        private void Log(object message)
        {
            if (!_showLogs) return;
            Debug.Log($"[{GetType().Name}] : {message}", this);
        }
    }
}