using System;

namespace JUtils
{
    public sealed class EventListener : BaseEventListener<EventChannel, EventChannel.Empty>
    {
        private event Action myListeners;


        public void AddListener(Action listener)
        {
            myListeners += listener;
        }


        public void RemoveListener(Action listener)
        {
            myListeners -= listener;
        }


        private void Awake()
        {
            AddListener(() => myListeners?.Invoke());
        }
    }
}