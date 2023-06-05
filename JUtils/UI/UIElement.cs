using System;
using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        protected VisualElement element       { get; set; }
        protected bool          isInitialized { get; private set; }
        public    bool          isActive      { get; private set; }
        
        protected abstract void OnInitialize();
        protected abstract void OnActivate();
        protected abstract void OnDeactivate();


        public void TryInitialize()
        {
            if (isInitialized) return;
            isInitialized = true;
            
            OnInitialize();
        }


        public virtual void Activate(VisualElement element)
        {
            if(isActive) return;
            isActive = true;
            gameObject.SetActive(true);
            
            TryInitialize();
            
            this.element = element;
            OnActivate();
        }
        

        public virtual void Deactivate()
        {
            if (!isActive) return;
            isActive = false;
            
            gameObject.SetActive(false);
            OnDeactivate();
        }


        protected virtual void Awake()
        {
            if (!isActive) {
                gameObject.SetActive(false);
            } 
        }
    }
    
    public abstract class UIElement<T> : UIElement where T : VisualElement
    {
        protected new T element => base.element as T;


        public virtual void Activate(T element)
        {
            base.Activate(element);
        }
        

        public override void Activate(VisualElement element)
        {
            if (element is not T castedElement) {
                throw new Exception($"VisualElement was not of type {typeof(T)}");
            }
            
            base.Activate(castedElement);
        }
    }
}
