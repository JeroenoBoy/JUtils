using UnityEditor;
using UnityEngine;



namespace JUtils.Attributes
{
    public class CustomName : PropertyAttribute
    {
        private readonly string _name;


        public CustomName(string name)
        {
            this._name = name;
        }
        
        
        #if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(CustomName))]
        private class NameEditor : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                label.text = ((CustomName)attribute)._name;
                EditorGUI.PropertyField(position, property, label);
            }
        }
        #endif
    }
}
