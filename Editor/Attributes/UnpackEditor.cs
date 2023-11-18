using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(UnpackAttribute))]
    public class UnpackEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.Generic) {
                Debug.LogError($"property ${property.propertyPath} must be generic!");
                return new VisualElement();
            }

            VisualElement root = new();
            SerializedProperty iterator = property.Copy();
            iterator.NextVisible(true);
            int depth = iterator.depth;
            do {
                root.Add(new PropertyField(iterator));
            } while (iterator.NextVisible(false) && iterator.depth == depth);
            
            return root;
        }
    }
}