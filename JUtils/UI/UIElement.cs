using System;
using System.Collections.Generic;
using System.Linq;
using JUtils.Extensions;
using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.UI
{
    /// <summary>
    /// Generic UIElement class, used with UIToolkit to add interactivity and binding for VisualElements
    /// </summary>
    public abstract class UIElement : MonoBehaviour
    {
        protected VisualElement element       { get; set; }
        protected bool          isInitialized { get; private set; }
        public    bool          active      { get; internal set; }
        public    UIElement     parent        { get; private set; }
        
        /// <summary>
        /// Prefer using <see cref="AddChild(UIElement, bool)"/> and <see cref="RemoveChild"/> to add / remove children from this UIElement
        /// </summary>
        public List<UIElementChildData> children { get; private set; } = new ();
        
        
        #region Options

        /// <summary>
        /// When set to true, this gameObject.SetActive() will be called when the element is activated
        /// </summary>
        protected virtual bool setGoBasedOnActive => true;

        /// <summary>
        /// Automatically querry the elements and activate them
        /// </summary>
        protected virtual bool autoEnableElements => true;

        /// <summary>
        /// Autometically find the children of this object
        /// </summary>
        protected virtual bool autoFindChildren => true;

        protected virtual string defaultQuery => name;

        #endregion


        private bool _didActivateOnce;


        public void TryInitialize()
        {
            if (isInitialized) return;
            isInitialized = true;

            if (autoFindChildren) ReFindChildElements();
            
            OnInitialize();
        }


        /// <summary>
        /// Activate this UIElement and its children
        /// </summary>
        /// <param name="element">The root visual element of this UIElement</param>
        public virtual void Activate(VisualElement element)
        {
            if(active) return;
            _didActivateOnce = true;
            if (setGoBasedOnActive) gameObject.SetActive(true);
            
            TryInitialize();
            
            active = true;
            this.element = element;
            if (autoEnableElements) ActivateAllChildren();
            OnActivate();
        }
        

        /// <summary>
        /// Deactivates this UIElement and its children
        /// </summary>
        public virtual void Deactivate()
        {
            if (!active) return;
            
            if (setGoBasedOnActive) gameObject.SetActive(false);
            if (autoEnableElements) DeactivateAllChildren();
            
            OnDeactivate();
            active = false;
        }


        /// <summary>
        /// Create a new GameObject and add the component of type T to it
        /// </summary>
        public T AddUIElement<T>(string name = null) where T : UIElement
        {
            GameObject newObj = new GameObject(name ?? nameof(T));
            T uiElement = newObj.AddComponent<T>();
            newObj.transform.parent = transform;
            AddChild(uiElement);
            return uiElement;
        }


        /// <summary>
        /// This gets called before <see cref="OnActivate"/> has been called for the first time
        /// </summary>
        protected virtual void OnInitialize() {}
        protected virtual void OnActivate() {}
        protected virtual void OnDeactivate() {}
        
        
        /// <summary>
        /// Returns true is the element is active or has been activated
        /// </summary>
        protected bool TryActivate(UIElement element)
        {
            if (!active || !autoEnableElements) return false;
            if (!TryGetChildData(element, out UIElementChildData childData)) return false;
            if (string.IsNullOrEmpty(childData.query)) return false;
            
            if (element.active) return true;
            element.Activate(this.element.Q(childData.query));
            return true;
        }


        /// <summary>
        /// Returns true is the element is active or has been activated
        /// </summary>
        protected bool TryActivate(UIElementChildData childData)
        {
            if (!active) return false;
            if (childData.element.parent != this) return false;
            if (string.IsNullOrEmpty(childData.query)) return false;
            
            if (childData.element.active) return true;
            childData.element.Activate(element.Q(childData.query));
            return true;
        }


        /// <summary>
        /// Automatically find all direct children of this element
        /// </summary>
        protected void ReFindChildElements(bool clear = true)
        {
            if (clear) RemoveAllChildren();

            foreach (UIElement element in this.GetComponentsInDirectChildren<UIElement>()) {
                AddChild(element);
            }
        }
        

        /// <summary>
        /// Activate all children this element has
        /// </summary>
        protected void ActivateAllChildren()
        {
            if (!active) return;

            for (int i = children.Count; i --> 0;) {
                TryActivate(children[i]);
            }
        }


        /// <summary>
        /// Deactivates all active children
        /// </summary>
        protected void DeactivateAllChildren()
        {
            if (!active) return;
            
            for (int i = children.Count; i --> 0;) {
                UIElementChildData childData = children[i];
                childData.element.Deactivate();
            }
        }

        
        /// <summary>
        /// Add a element to the children of this element with its default query
        /// </summary>
        protected bool AddChild(UIElement element, bool tryAutoActivate = true)
        {
            return AddChild(element.defaultQuery, element, tryAutoActivate);
        }


        /// <summary>
        /// Add a child element to this state and automatically enable it
        /// </summary>
        protected bool AddChild(string query, UIElement element, bool tryAutoActivate = true)
        {
            if (element.parent != null) return false;
            if (element == this) return false;
            if (ContainsChild(element)) return true;
            
            UIElementChildData newChildData = new()
            {
                element = element,
                query = query
            };
            
            children.Add(newChildData);

            element.parent = this;
            if (tryAutoActivate) TryActivate(newChildData);
            
            return true;
        }


        /// <summary>
        /// Remove a UIElement from the children of this element
        /// </summary>
        protected bool RemoveChild(UIElement element)
        {
            if (!TryGetChildData(element, out UIElementChildData childData)) return false;
            if (!children.Remove(childData)) return false;

            if (element.active) {
                element.Deactivate();
            }

            element.parent = null;
            return true;
        }


        /// <summary>
        /// Remove all elements from this element and deactivate them as well
        /// </summary>
        protected void RemoveAllChildren()
        {
            for (int i = children.Count; i --> 0;) {
                UIElement element = children[i].element;

                if (element.active) {
                    element.Deactivate();
                }
                
                element.parent = null;
            }
        }


        /// <summary>
        /// Try get an element's child data from this component
        /// </summary>
        protected bool TryGetChildData(UIElement element, out UIElementChildData childData)
        {
            for (int i = children.Count; i --> 0;) {
                if (children[i].element != element) continue;
                
                childData = children[i];
                return true;
            }

            childData = default;
            return false;
        }


        /// <summary>
        /// Check with a predicate if this element contains a child
        /// </summary>
        protected bool ContainsChild(Predicate<UIElementChildData> predicate)
        {
            for (int i = children.Count; i --> 0;) {
                if (predicate(children[i])) {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Check if this element contains the element
        /// </summary>
        protected bool ContainsChild(UIElement element) => ContainsChild(e => e.element == element);


        protected virtual void Awake()
        {
            if (!_didActivateOnce && setGoBasedOnActive) {
                gameObject.SetActive(false);
            } 
        }
    }
    
    
    
    /// <summary>
    /// UIElement where <see cref="element"/> is casted as a different VisualElement
    /// </summary>
    public abstract class UIElement<T> : UIElement where T : VisualElement
    {
        protected new T element => base.element as T;


        public virtual void Activate(T element)
        {
            base.Activate(element);
        }
        

        public override void Activate(VisualElement element)
        {
            if (element is null) {
                throw new Exception("VisualElement is null!");
            }
            
            if (element is not T castedElement) {
                throw new Exception($"VisualElement was not of type {typeof(T)}");
            }
            
            base.Activate(castedElement);
        }
    }



    /// <summary>
    /// Data structure representing a <see cref="UIElement"/>'s child
    /// </summary>
    public struct UIElementChildData
    {
        public UIElement element;
        public string    query;
    }
}
