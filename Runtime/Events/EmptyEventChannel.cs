using UnityEngine;

namespace JUtils
{
    [CreateAssetMenu(menuName = "JUtils/Events/Event Channel")]
    public sealed class EmptyEventChannel : EventChannel<EmptyEventChannel.Empty>
    {
        public struct Empty { }
    }
}