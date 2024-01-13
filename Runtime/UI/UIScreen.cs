using UnityEngine.UIElements;

namespace JUtils.UI
{
    public interface IUIScreen
    {
        public string screenId { get; }
        public bool isShowing { get; }
        public void Initialize(VisualElement newRootElement);
        public void Show();
        public void Hide();
    }


    public abstract class UIScreen<T> : SingletonBehaviour<T>, IUIScreen where T : UIScreen<T>
    {
        public bool isShowing { get; private set; }
        public abstract string screenId { get; }

        protected virtual bool hideOnInitialize => true;
        protected virtual bool setActiveBasedOnShowStatus => true;

        protected UIDocument uiDocument => uiScreenManager.uiDocument;
        protected UIScreenManager uiScreenManager { get; private set; }
        protected VisualElement rootElement { get; private set; }


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


        public void Show()
        {
            if (isShowing) return;

            isShowing = true;

            if (setActiveBasedOnShowStatus) {
                gameObject.SetActive(true);
            }

            OnShow();
        }


        public void Hide()
        {
            if (!isShowing) return;
            OnHide();
            isShowing = false;

            if (setActiveBasedOnShowStatus) {
                gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// Get called when the screen either first shows or the <see cref="Initialize"/> has been called
        /// </summary>
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
        }
    }
}