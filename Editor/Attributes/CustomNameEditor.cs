using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(CustomNameAttribute))]
    public class CustomNameEditor : PropertyDrawer
    {
        private new CustomNameAttribute attribute => base.attribute as CustomNameAttribute;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();
            root.AddToClassList("panel");
            root.Add(new PropertyField(property, attribute.name));
            return root;
        }
    }
}