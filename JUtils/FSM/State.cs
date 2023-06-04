using System;
using System.Runtime.CompilerServices;
using JUtils.Extensions;
using UnityEngine;



namespace JUtils.FSM
{
    /// <summary>
    /// A simple state that can be driven by <see cref="StateMachine"/>
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        [SerializeField] private bool setEnabledBasedOnActive = true;

        public event Action<State> OnStateActivate;
        public event Action<State> OnStateDeactivate;

        public bool         IsActive     { get; private set; }
        public StateMachine StateMachine { get; internal set; }

        /// <summary>
        /// Get the amount of seconds that this state is active. Returns 0 if the state is not active
        /// </summary>
        public float TimeInState => IsActive ? Time.time - _timeEnteredState : -1f;

        /// <summary>
        /// Get the amount of unscaled seconds that this state is active. Returns 0 if the state is not active
        /// </summary>
        public float UnscaledTimeInState => IsActive ? Time.unscaledTime - _unscaledTimeEnteredState : -1f;
        
        protected StateData Data { get; private set; }
        

        private float _timeEnteredState;
        private float _unscaledTimeEnteredState;
        
       
        /// <summary>
        /// Internal function of activating the state
        /// </summary>
        internal virtual bool ActivateState(StateData data)
        {
            try {
                _timeEnteredState         = Time.time;
                _unscaledTimeEnteredState = Time.unscaledTime;

                Data = data;
                
                gameObject.SetActive(true);
                IsActive = true;
                OnActivate();
                OnStateActivate?.Invoke(this);
                return true;
            }
            catch (Exception e) {
                Debug.LogException(e);
                Coroutines.RunNextFrame(Deactivate); // Run next frame to avoid stack overflow exceptions
                return false;
            }
        }


        /// <summary>
        /// Internal function for deactivating the state
        /// </summary>
        internal virtual void DeactivateState()
        {
            try {
                OnDeactivate();
                
                if (setEnabledBasedOnActive) {
                    gameObject.SetActive(false);
                }
                
                IsActive = false;
                Data = null;
                OnStateDeactivate?.Invoke(this);
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }


        /// <summary>
        /// Deactivate this state and make the state-machine continue its queue
        /// </summary>
        protected void Deactivate()
        {
            if (!IsActive) return;
            StateMachine.ContinueQueue();
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
        protected StateRef<T> Ref<T>() where T : State => new ();


        /// <summary>
        /// Only runs the Update() function when the state is active
        /// </summary>
        protected virtual void ActiveUpdate()
        {
        }


        protected virtual void Awake()
        {
            if (!IsActive && setEnabledBasedOnActive) gameObject.SetActive(false);
        }


        protected virtual void Update()
        {
            if (IsActive) ActiveUpdate();
        }
    }

    
    
    /// <summary>
    /// State with 1 type-safe arguments
    /// </summary>
    public abstract class State<T> : State
    {
        protected override void OnActivate()
        {
            T a = Data.Get<T>(0);
            OnActivate(a);
        }


        protected abstract void OnActivate(T a);
    }



    /// <summary>
    /// State with 2 type-safe arguments
    /// </summary>
    public abstract class State<T1, T2> : State
    {
        protected override void OnActivate()
        {
            T1 a = Data.Get<T1>(0);
            T2 b = Data.Get<T2>(0);
            OnActivate(a, b);
        }


        protected abstract void OnActivate(T1 a, T2 b);
    }
    
    
    
    /// <summary>
    /// State with 3 type-safe arguments
    /// </summary>
    public abstract class State<T1, T2, T3> : State
    {
        protected override void OnActivate()
        {
            T1 a = Data.Get<T1>(0);
            T2 b = Data.Get<T2>(0);
            T3 c = Data.Get<T3>(0);
            OnActivate(a, b, c);
        }


        protected abstract void OnActivate(T1 a, T2 b, T3 c);
    }
}
