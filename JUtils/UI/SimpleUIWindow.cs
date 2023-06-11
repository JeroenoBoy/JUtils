using JUtils.Internal;
using JUtils.Singletons;
using UnityEngine;
using UnityEngine.UIElements;



namespace JUtils.UI
{
    /// <summary>
    /// Simple base UI window class for referencing windows by name, does not provide any functionality by itself.
    /// </summary>
    public sealed class SimpleUIWindow : UIElement
    {
        /// <summary>
        /// Get the serialized dictionary of all current windows
        /// </summary>
        public static SerializableDictionary<string, SimpleUIWindow> windows => UIWindowStore.GetOrCreate();


        /// <summary>
        /// Try get a specific UI window via name
        /// </summary>
        public static bool TryGet(string name, out SimpleUIWindow window)
        {
            return windows.TryGetValue(name, out window);
        }


        /// <summary>
        /// Get a specific UI window by name
        /// </summary>
        public static SimpleUIWindow Get(string name)
        {
            return TryGet(name, out SimpleUIWindow window) ? window : null;
        }


        /// <summary>
        /// Shorthand for SimpleUIWindow.Get(name).Activate()
        /// </summary>
        public static void Activate(string name)
        {
            Get(name).Activate();
        }


        /// <summary>
        /// Shorthand for SimpleUIWindow.Get(name).Deactivate()
        /// </summary>
        public static void Deactivate(string name)
        {
            Get(name).Deactivate();
        }
        
        
        
        public UIDocument uiDocument  { get; private set; }
        
        
        /// <summary>
        /// Activate the window based on the given state
        /// </summary>
        public void SetActive(bool active)
        {
            if (active) Activate();
            else Deactivate();
        }


        /// <summary>
        /// Activate the UI window
        /// </summary>
        public void Activate()
        { 
            if (active) { return; }
            
            gameObject.SetActive(true);
            base.Activate(uiDocument.rootVisualElement);
        }


        /// <summary>
        /// element param will not get used!!
        /// </summary>
        public override void Activate(VisualElement element) => Activate();

        
        [SerializeField] private string overrideWindowName;

        
        protected override void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            windows.Add(string.IsNullOrEmpty(overrideWindowName) ? name : overrideWindowName, this);
            base.Awake();
        }


        private void OnDestroy()
        {
            windows.Remove(string.IsNullOrEmpty(overrideWindowName) ? name : overrideWindowName);
        }
        
        
       
        /// <summary>
        /// Class responsible for holding all the references
        /// </summary>
        internal sealed class UIWindowStore : SingletonBehaviour<UIWindowStore>
        {
            public static SerializableDictionary<string, SimpleUIWindow> GetOrCreate()
            {
                return !exists ? JUtilsObject.Add<UIWindowStore>().windowsStore : instance.windowsStore;
            }

            public SerializableDictionary<string, SimpleUIWindow> windowsStore = new ();
        }
    }
}
