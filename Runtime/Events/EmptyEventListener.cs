<<<<<<< HEAD
﻿namespace JUtils
=======
using System;

namespace JUtils
>>>>>>> develop
{
    public sealed class EmptyEventListener : EventListener<EmptyEventChannel, EmptyEventChannel.Empty>
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