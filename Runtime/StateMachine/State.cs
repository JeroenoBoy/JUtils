using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A simple state that can be driven by <see cref="stateMachine"/>
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        [SerializeField] private bool _setEnabledBasedOnActive = true;

        public event Action<State> onStateActivate;
        public event Action<State> onStateDeactivate;

        public bool isActive { get; private set; }

        /// <summary>
        /// Is set to true while the state is activating
        /// </summary>
        public bool isActivating { get; private set; }

        /// <summary>
        /// Is set to true while the state is deactivating
        /// </summary>
        public bool isDeactivating { get; private set; }

        /// <summary>
        /// The reference of this state's state machine
        /// </summary>
        public StateMachine stateMachine { get; internal set; }

        /// <summary>
        /// Get the amount of seconds that this state is active. Returns 0 if the state is not active
        /// </summary>
        public float timeInState => isActive ? Time.time - _timeEnteredState : -1f;

        /// <summary>
        /// Get the amount of unscaled seconds that this state is active. Returns 0 if the state is not active
        /// </summary>
        public float unscaledTimeInState => isActive ? Time.unscaledTime - _unscaledTimeEnteredState : -1f;

        protected StateData stateData { get; private set; }

        private float _timeEnteredState;
        private float _unscaledTimeEnteredState;

        /// <summary>
        /// Internal function of activating the state
        /// </summary>
        internal bool ActivateState(StateData data)
        {
            try {
                isActivating = true;
                _timeEnteredState = Time.time;
                _unscaledTimeEnteredState = Time.unscaledTime;

                stateData = data;

                gameObject.SetActive(true);
                isActive = true;
                OnActivate();
                onStateActivate?.Invoke(this);
                isActivating = false;
                return true;
            } catch (Exception e) {
                Debug.LogException(e);
                Coroutines.RunNextFrame(Deactivate); // Run next frame to avoid stack overflow exceptions
                isActivating = false;
                return false;
            }
        }

        /// <summary>
        /// Internal function for deactivating the state
        /// </summary>
        internal virtual void DeactivateState()
        {
            isDeactivating = true;

            try {
                OnDeactivate();

                if (_setEnabledBasedOnActive) {
                    gameObject.SetActive(false);
                }

                isActive = false;
                stateData = null;
                onStateDeactivate?.Invoke(this);
            } catch (Exception e) {
                Debug.LogException(e);
            }

            isDeactivating = false;
        }

        /// <summary>
        /// Deactivate this state and make the state-machine continue its queue
        /// </summary>
        protected void Deactivate()
        {
            if (!isActive) return;
            if (stateMachine == null) return;
            stateMachine.ContinueQueue();
        }

        /// <summary>
        /// Gets called when the state activates
        /// </summary>
        protected abstract void OnActivate();

        /// <summary>
        /// Gets called when the sate deactivates
        /// </summary>
        protected abstract void OnDeactivate();

        /// <summary>
        /// Used for making typed argument adding to states prettier
        /// </summary>
        /// <typeparam name="T">The type we want the ref from</typeparam>
        /// <example><code>
        /// StateMachine.AddToQueue(Ref&#60;TeleportState>, location);
        /// </code></example>>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected StateRef<T> Ref<T>() where T : State
        {
            return new StateRef<T>();
        }

        /// <summary>
        /// Only runs the Update() function when the state is active
        /// </summary>
        protected virtual void ActiveUpdate() { }

        protected virtual void Awake()
        {
            if (!isActive && _setEnabledBasedOnActive) gameObject.SetActive(false);
        }

        protected virtual void Update()
        {
            if (isActive) ActiveUpdate();
        }
    }


    /// <summary>
    /// State with 1 type-safe arguments
    /// </summary>
    public abstract class State<T> : State
    {
        protected override void OnActivate()
        {
            T param = stateData.Get<T>(0);
            OnActivate(param);
        }

        protected abstract void OnActivate(T param);
    }


    /// <summary>
    /// State with 2 type-safe arguments
    /// </summary>
    public abstract class State<T1, T2> : State
    {
        protected override void OnActivate()
        {
            T1 param1 = stateData.Get<T1>(0);
            T2 param2 = stateData.Get<T2>(1);
            OnActivate(param1, param2);
        }

        protected abstract void OnActivate(T1 param1, T2 param2);
    }


    /// <summary>
    /// State with 3 type-safe arguments
    /// </summary>
    public abstract class State<T1, T2, T3> : State
    {
        protected override void OnActivate()
        {
            T1 param1 = stateData.Get<T1>(0);
            T2 param2 = stateData.Get<T2>(1);
            T3 param3 = stateData.Get<T3>(2);
            OnActivate(param1, param2, param3);
        }

        protected abstract void OnActivate(T1 param1, T2 param2, T3 param3);
    }
}