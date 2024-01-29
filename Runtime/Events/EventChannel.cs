using System;
using UnityEngine;

namespace JUtils
{
    [CreateAssetMenu(menuName = "JUtils/Events/Event Channel")]
    public sealed class EventChannel : BaseEventChannel<EventChannel.Empty>
    {
        private event Action listeners;


        public void AddListener(Action listener)
        {
            listeners += listener;
        }


        public void RemoveListener(Action listener)
        {
            listeners -= listener;
        }


        public override void InvokeUnsafe(Empty argument)
        {
            listeners?.Invoke();
            base.InvokeUnsafe(argument);
        }


        public struct Empty { }
    }
}