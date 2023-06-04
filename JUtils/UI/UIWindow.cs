using System;
using JUtils.Singletons;
using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.UI
{
    /// <summary>
    /// Base component for creating UI with JUtils, this should be the entry point fot a UIWindow
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public abstract class UIWindow<T> : SingletonBehaviour<T> where T : UIWindow<T>
    {
        [SerializeField] private bool enabledBasedOnActive = true;

        public    bool          active        { get; private set; }
        protected UIDocument    uiDocument    { get; private set; }
        protected VisualElement rootElement   { get; private set; }
        protected bool          isInitialized { get; private set; }


        /// <summary>
        /// Start initializing this UIWindow
        /// </summary>
        /// <returns>True if the window has been initialized</returns>
        public bool Initialize()
        {
            if (isInitialized) return true;

            try {
                OnInitialize();
                isInitialized = true;
            }
            catch (Exception e) {
                Debug.LogWarning($"Error happened while initializing {GetType()}", this);
                Debug.LogException(e);
            }

            return isInitialized;
        }


        /// <summary>
        /// Set the window active based on the active parameter
        /// </summary>
        public void SetActive(bool active)
        {
            if (active) Activate();
            else Deactivate();
        }
        

        /// <summary>
        /// Activate this UIWindow
        /// </summary>
        public void Activate()
        {
            if (active) return;
            if (!Initialize()) return;

            active = true;

            if (enabledBasedOnActive) gameObject.SetActive(true);

            uiDocument.enabled = true;
            rootElement        = uiDocument.rootVisualElement;

            try {
                OnActivate();
            }
            catch (Exception e) {
                Debug.LogWarning($"Error happened while activating {GetType()}, deactivating", this);
                Debug.LogException(e);
                Deactivate();
            }
        }


        /// <summary>
        /// Deactivate this UIWindow
        /// </summary>
        public void Deactivate()
        {
            if (!active) return;
            active = false;

            if (enabledBasedOnActive) gameObject.SetActive(false);

            uiDocument.enabled = false;

            try {
                OnDeactivate();
            }
            catch (Exception e) {
                Debug.LogWarning($"Error happened while deactivating {GetType()}", this);
                Debug.LogException(e);
            }
        }
        

        protected abstract void OnInitialize();
        protected abstract void OnActivate();
        protected abstract void OnDeactivate();


        protected override void Awake()
        {
            base.Awake();
            uiDocument  = GetComponent<UIDocument>();
            uiDocument.enabled = false;
            if (enabledBasedOnActive) gameObject.SetActive(false);
        }
    }
}