using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils.UI
{
    /// <summary>
    /// This component will give the UI screens their references. Also makes the ui screens reinitialize when the document has been reloaded
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class UIScreenManager : MonoBehaviour
    {
        public UIDocument uiDocument { get; private set; }

        private bool _isInitialized;
        private readonly Dictionary<string, IUIScreen> _screens = new();


        /// <summary>
        /// Register a new UI screen
        /// </summary>
        /// <param name="uiScreen"></param>
        public void Register([NotNull] IUIScreen uiScreen)
        {
            _screens.Add(uiScreen.screenId, uiScreen);
            if (_isInitialized) {
                uiScreen.Initialize(uiDocument.rootVisualElement.Q(uiScreen.screenId));
            }
        }


        public void Unregister(IUIScreen uiScreen)
        {
            _screens.Remove(uiScreen.screenId);
        }


        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
        }


        private void OnEnable()
        {
            _isInitialized = true;
            foreach ((string name, IUIScreen screen) in _screens) {
                screen.Initialize(uiDocument.rootVisualElement.Q(name));
            }
        }


        private void OnDisable()
        {
            _isInitialized = false;
        }
    }
}