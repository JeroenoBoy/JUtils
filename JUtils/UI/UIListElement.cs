using System;
using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.UI
{
    public abstract class UIListElement<TData, TElement> : UIElement<TElement> where TElement : VisualElement
    {
        [SerializeField] private VisualTreeAsset _visualElementAsset;
        
        public TData data { get; set; }

        protected virtual FindMethod method            { get; set; }
        protected virtual bool       autoFindTreeAsset => true;
        
            
        protected abstract void OnActivate(TData data);


        public void Activate(VisualElement parent, TData data)
        {
            if (active) return;
            
            this.data = data;
            VisualElement newElement = CreateElement(parent);
            parent.Add(newElement);
            Activate(newElement);
        }


        protected override void OnActivate()
        {
            OnActivate(data);
        }


        protected override void OnDeactivate()
        {
            data = default;
        }


        protected override void Awake()
        {
            base.Awake();

            if (autoFindTreeAsset && _visualElementAsset != null) {
                method = FindMethod.Field;
            }
            else {
                method = FindMethod.Unknown;
            }
        }


        public VisualElement CreateElement(VisualElement parent)
        {
            if ((autoFindTreeAsset && method == FindMethod.Unknown) || (method == FindMethod.Hierarchy && _visualElementAsset == null)) {
                _visualElementAsset = FindAsset(parent);
                if (_visualElementAsset != null) method = FindMethod.Hierarchy;
            }

            return method switch
            {
                FindMethod.Field     => _visualElementAsset.CloneTree(),
                FindMethod.Hierarchy => _visualElementAsset.CloneTree(),
                _                    => throw new ArgumentOutOfRangeException()
            };
        }


        private VisualTreeAsset FindAsset(VisualElement parent)
        {
            for (int i = 0; i < parent.childCount; i++) {
                VisualElement targetParent = parent.ElementAt(i);
                VisualElement target = targetParent.ElementAt(0);
                if (target == null || !target.visualTreeAssetSource) continue;
                
                targetParent.SetDisplay(false);
                return target.visualTreeAssetSource;
            }

            return null;
        }



        public enum FindMethod
        {
            Unknown,
            Field,
            Hierarchy
        }
    }


    public abstract class UIListElement<T> : UIListElement<T, VisualElement>
    {
    }
}
