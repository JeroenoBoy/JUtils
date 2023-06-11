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
    public abstract class UIWindow<T> : UIElement, ISingleton<T> where T : UIWindow<T>
    {
        private static T _instance;
        public static  T instance => _instance ??= SingletonManager.GetSingleton<T>();

        public UIDocument    uiDocument  { get; private set; }


        public void SetActive(bool active)
        {
            if (active) Activate();
            else Deactivate();
        }


        public void Activate()
        { 
            if (active) { return; }
            
            gameObject.SetActive(true);
            base.Activate(uiDocument.rootVisualElement);
        }


        /// <summary>
        /// element param will not get used!!
        /// </summary>
        public override void Activate(VisualElement element) => Activate();


        protected override void Awake()
        {
            if (this is not T) {
                throw new Exception($"{GetType()} does not match typeof {nameof(T)} ({typeof(T)})");
            }
            
            if (!SingletonManager.SetSingleton(this)) {
                Destroy(this); 
                Debug.LogError("Instance already exists!");
                return;
            }
            
            _instance = this as T;
            
            uiDocument = GetComponent<UIDocument>();
            base.Awake();
        }
    }
}