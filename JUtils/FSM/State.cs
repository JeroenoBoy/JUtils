using System;
using JUtils.Extensions;
using UnityEngine;



namespace JUtils.FSM
{
    public abstract class State : MonoBehaviour
    {
        [SerializeField] private bool setEnabledBasedOnActive = true;

        public event Action<State> OnStateActivate;
        public event Action<State> OnStateDeactivate;

        public bool         IsActive            { get; private set; }
        public float        TimeInState         => IsActive ? Time.time - _timeEnteredState : -1f;
        public float        UnscaledTimeInState => IsActive ? Time.unscaledTime - _unscaledTimeEnteredState : -1f;
        public StateMachine StateMachine        { get; internal set; }
        public StateData    Data                { get; internal set; }

        private float _timeEnteredState;
        private float _unscaledTimeEnteredState;
        
        internal void ActivateState()
        {
            try {
                _timeEnteredState         = Time.time;
                _unscaledTimeEnteredState = Time.unscaledTime;
                
                gameObject.SetActive(true);
                IsActive = true;
                OnActivate();
                OnStateActivate?.Invoke(this);
            }
            catch (Exception e) {
                Debug.LogException(e);
                Coroutines.RunNextFrame(Deactivate); // Run next frame to avoid stack overflow exceptions
            }
        }


        internal void DeactivateState()
        {
            try {
                if (setEnabledBasedOnActive) {
                    gameObject.SetActive(false);
                }
                
                IsActive = false;
                OnDeactivate();
                OnStateDeactivate?.Invoke(this);
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }


        protected void Deactivate()
        {
            if (!IsActive) return;
            StateMachine.ContinueQueue();
        }
        
        
        protected abstract void OnActivate();
        protected abstract void OnDeactivate();


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



    public abstract class State<T> : State
    {
        protected override void OnActivate()
        {
            T a = Data.Get<T>(0);
            OnActivate(a);
        }


        protected abstract void OnActivate(T a);
    }



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
