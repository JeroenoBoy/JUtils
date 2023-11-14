using UnityEditor;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxInt))]
    public class MinMaxIntEditor : BaseMinMaxEditor
    {
        protected override BindableElement CreateElement(string name)
        {
            return new IntegerField(name);
        }
    }
}