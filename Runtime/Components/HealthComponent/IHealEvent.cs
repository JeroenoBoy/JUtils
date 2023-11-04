namespace JUtils
{
    /// <summary>
    /// Used to create more detailed heal events
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class Player : MonoBehaviour
    ///     {
    ///         private void OnHeal(MagicHealEvent healEvent)
    ///         {
    ///             // Reacting to the event
    ///         }
    ///
    ///         private void OnHeal(IHealEvent healEvent)
    ///         {
    ///             // Normal event handler
    ///         }
    ///     }
    /// }
    /// </code></example>
    public interface IHealEvent
    {
        int amount { get; set; }
    }
}
