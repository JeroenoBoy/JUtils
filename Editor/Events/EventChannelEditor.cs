using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BaseEventChannel<>), true)]
    public class EventChannelEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _eventChannelTree;

        private VisualElement _warningElement;


        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            VisualElement tree = _eventChannelTree.Instantiate();
            tree.Bind(serializedObject);

            _warningElement = tree.Q<VisualElement>("Warning");
            UpdateWarningElement();

            root.Add(tree);
            return root;
        }


        private void UpdateWarningElement()
        {
            string path = AssetDatabase.GetAssetPath(serializedObject.targetObject);
            _warningElement.visible = !path.Contains("Resources/Events");
        }
    }
}