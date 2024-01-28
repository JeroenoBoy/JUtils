using System.Threading;
using UnityEngine.UIElements;

namespace JUtils.UI
{
    /// <summary>
    /// An visual element that generates a cancellation token for async calls
    /// </summary>
    public abstract class UpdatingVisualElement : VisualElement
    {
        public CancellationToken cancellationToken => _cancellationTokenSource.Token;

        private CancellationTokenSource _cancellationTokenSource;


        public UpdatingVisualElement()
        {
            RegisterCallback<AttachToPanelEvent>(HandleAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(HandleDetatchFromPanel);
        }


        private void HandleAttachToPanel(AttachToPanelEvent evt)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }


        private void HandleDetatchFromPanel(DetachFromPanelEvent evt)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}