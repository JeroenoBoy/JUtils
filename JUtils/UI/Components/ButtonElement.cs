using UnityEngine.UIElements;



namespace JUtils.UI
{
    /// <summary>
    /// This class provides callbacks for working with button elements
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class QuitGameButton : ButtonElement
    ///     {
    ///         public override void OnClick(ClickEvent e)
    ///         {
    ///             Debug.Log("Quitting!");
    ///             Application.Quit();
    ///         }
    ///     }
    /// }
    /// </code></example>
    public abstract class ButtonElement : UIElement<Button>
    {
        public override void Activate(VisualElement element)
        {
            if (active) return;
            base.Activate(element);

            Button button = this.element;
            button.RegisterCallback<ClickEvent>(OnClick);
            button.RegisterCallback<FocusInEvent>(OnFocusIn);
            button.RegisterCallback<FocusOutEvent>(OnFocusOut);
        }


        protected abstract void OnClick(ClickEvent e);
        protected virtual void OnFocusIn(FocusInEvent e) {}
        protected virtual void OnFocusOut(FocusOutEvent e) {}
    }
}
