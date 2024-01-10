using System;
using UnityEngine;

namespace JUtils.Events
{
    [CreateAssetMenu(menuName = "JUtils/Events/Event Channel")]
    public sealed class EmptyEventChannel : EventChannel<EmptyEventChannel.Empty>
    {
        public struct Empty { }
    }
}