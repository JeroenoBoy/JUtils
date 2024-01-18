using UnityEditor;
using UnityEngine.UIElements;

namespace JUtils.Editor
{
    [CustomPropertyDrawer(typeof(MinMax))]
    public class MinMaxEditor : BaseMinMaxEditor
    {
        protected override BindableElement CreateElement(string name)
        {
            return new FloatField(name);
        }
    }
}