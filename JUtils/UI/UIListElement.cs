using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.UI
{
    public abstract class UIListElement<TData, TElement> : UIElement<TElement> where TElement : VisualElement
    {
        [SerializeField] private VisualTreeAsset _visualElementAsset;
        
        
        public TData data { get; set; }
        
        protected abstract void OnActivate(TData data);


        public void Activate(VisualElement parent, TData data)
        {
            if (active) return;
            
            this.data = data;
            VisualElement newElement = _visualElementAsset.CloneTree().contentContainer;
            parent.Add(newElement);
            Activate(newElement);
        }


        protected override void OnActivate()
        {
            OnActivate(data);
        }


        public override void Deactivate()
        {
            base.Deactivate();
            data = default;
        }
    }


    public abstract class UIListElement<T> : UIListElement<T, VisualElement>
    {
    }
}
