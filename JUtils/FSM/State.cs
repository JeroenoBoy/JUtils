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

        public bool         IsActive     { get; internal set; }
        public StateMachine StateMachine { get; internal set; }


        internal void ActivateState()
        {
            try {
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
}
