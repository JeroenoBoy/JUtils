using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.UI
{
    public abstract class UIList<T> : UIElement<VisualElement>
    {
        [SerializeField] protected UIListElement<T> listElement;

        public List<UIListElement<T>>        elements       { get; } = new ();
        public IEnumerable<UIListElement<T>> activeElements => elements.Where(x => x.active);

        public T[] data { get; private set; } = Array.Empty<T>();


        protected override bool autoFindChildren   => false;
        protected override bool autoEnableElements => false;


        public void Expand(int count)
        {
            for (int i = count; i --> 0;) {
                elements.Add(Instantiate(listElement.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<UIListElement<T>>());
                AddChild(elements[^1], false);
            }
        }
        
        
        public void SetData(IEnumerable<T> data)
        {
            if (!active) {
                Debug.LogError("Please activate this UIElement before setting the data!", this);
                return;
            }
            
            T[] arrayData = data is T[] tArr ? tArr : data.ToArray();
            OnDataChanged(arrayData);
            this.data = arrayData;
        }


        protected virtual void OnDataChanged(T[] newData)
        {
            foreach (UIListElement<T> element in activeElements) {
                element.Deactivate();
            }

            if (elements.Count < newData.Length) {
                Expand(newData.Length - elements.Count);
            }

            for (int i = 0; i < newData.Length; i++) {
                elements[i].Activate(element, newData[i]);
            }
        }


        public void Activate(ListView element, IEnumerable<T> data)
        {
            if (active) { return; }
            Activate(element);
            SetData(data);
        }


        protected override void OnDeactivate()
        {
            SetData(Array.Empty<T>());
        }
    }
}
