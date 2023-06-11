using UnityEngine.UIElements;



namespace JUtils.UI
{
    /// <summary>
    /// This class provides callbacks for working with textfield elements
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class NameField : InputElement
    ///     {
    ///         public override void OnClick(ClickEvent e)
    ///         {
    ///             Debug.Log("Quitting!");
    ///             Application.Quit();
    ///         }
    ///     }
    /// }
    /// </code></example>
    public abstract class TextFieldElement : UIElement<TextField>
    {
        public override void Activate(VisualElement element)
        {
            if (active) return;
            base.Activate(element);

            TextField textField = this.element;
            textField.RegisterCallback<NavigationSubmitEvent>(OnSubmit);
            textField.RegisterCallback<InputEvent>(OnInput);
            textField.RegisterCallback<FocusInEvent>(OnFocusIn);
            textField.RegisterCallback<FocusOutEvent>(OnFocusOut);
        }


        protected abstract void OnSubmit(NavigationSubmitEvent e);
        protected virtual  void OnInput(InputEvent e) { }
        protected virtual  void OnFocusIn(FocusInEvent e) { }
        protected virtual  void OnFocusOut(FocusOutEvent e) { }
    }
}
