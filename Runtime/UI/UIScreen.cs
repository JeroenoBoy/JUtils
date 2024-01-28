using System;
using System.Threading;
using UnityEngine.UIElements;

namespace JUtils.UI
{
    /// <summary>
    /// Interface that serves as the base UI screen for <see cref="UIScreenManager"/>
    /// </summary>
    public interface IUIScreen
    {
        public string screenId { get; }
        public bool isShowing { get; }
        public void Initialize(VisualElement newRootElement);
        public void Show();
        public void Hide();
    }


    /// <summary>
    /// <see cref="IUIScreen"/> singleton based implementation
    /// </summary>
    public abstract class UIScreen<T> : SingletonBehaviour<T>, IUIScreen where T : UIScreen<T>
    {
        public bool isShowing { get; private set; }
        public abstract string screenId { get; }

        /// <summary>
        /// This token gets cancelled when the screen starts hiding
        /// </summary>
        public CancellationToken showCancellationToken => _showCancellationToken.Token;

        /// <summary>
        /// This token gets cancelled when the screen starts showing
        /// </summary>
        public CancellationToken hideCancellationToken => _hideCancellationToken.Token;

        protected virtual bool hideOnInitialize => true;
        protected virtual bool setActiveBasedOnShowStatus => true;

        protected UIDocument uiDocument => uiScreenManager.uiDocument;
        protected UIScreenManager uiScreenManager { get; private set; }
        protected VisualElement rootElement { get; private set; }

        private CancellationTokenSource _showCancellationToken;
        private CancellationTokenSource _hideCancellationToken;


        /// <summary>
        /// Initialize the UI screen
        /// </summary>
        public void Initialize(VisualElement newRootElement)
        {
            rootElement = newRootElement;
            OnInitialize();

            if (hideOnInitialize) {
                newRootElement.visible = false;
            }

            if (isShowing) {
                Hide();
                Show();
            } else {
                gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// Show the UI screen
        /// </summary>
        public void Show()
        {
            if (isShowing) return;
            _hideCancellationToken?.Cancel();
            isShowing = true;

            if (setActiveBasedOnShowStatus) {
                gameObject.SetActive(true);
            }

            _showCancellationToken = new CancellationTokenSource();
            OnShow();
        }


        /// <summary>
        /// Hide the UI screen
        /// </summary>
        public void Hide()
        {
            if (!isShowing) return;
            _showCancellationToken?.Cancel();
            _hideCancellationToken = new CancellationTokenSource();
            OnHide();
            isShowing = false;

            if (setActiveBasedOnShowStatus) {
                gameObject.SetActive(false);
            }
        }


        protected abstract void OnInitialize();


        protected abstract void OnShow();
        protected abstract void OnHide();


        protected override void Awake()
        {
            base.Awake();
            uiScreenManager = GetComponentInParent<UIScreenManager>();
            uiScreenManager.Register(this);
        }


        protected override void OnDestroy()
        {
            uiScreenManager.Unregister(this);
            _showCancellationToken?.Dispose();
            _hideCancellationToken?.Dispose();
        }
    }


    /// <summary>
    /// Show the UI screen with specific given data.
    /// </summary>
    public abstract class UIScreen<TScreen, TData> : UIScreen<TScreen> where TScreen : UIScreen<TScreen, TData>
    {
        public TData data { get; private set; }


        /// <summary>
        /// Marking as obselete because <see cref="Show(TData)"/> should be used
        /// </summary>
        [Obsolete("Use Show(TData) instead.")]
        public new void Show()
        {
            base.Show();
        }


        /// <summary>
        /// Shows the UI screen with the given data.
        /// </summary>
        public void Show(TData newData)
        {
            if (isShowing) return;
            data = newData;
            base.Show();
        }
    }
}