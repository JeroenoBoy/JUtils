namespace JUtils
{
    /// <summary>
    /// Used to create more detailed damage events
    /// </summary>
    /// <example><code lang="CSharp">
    /// namespace Example
    /// {
    ///     public class Player : MonoBehaviour
    ///     {
    ///         private void OnDamage(BulletDamageEvent bulletDamageEvent)
    ///         {
    ///             // Reacting to the event
    ///         }
    ///
    ///         private void OnDamage(IDamageEvent damageEvent)
    ///         {
    ///             // Normal event handler
    ///         }
    ///     }
    /// }
    /// </code></example>
    public interface IDamageEvent
    {
        int damage { get; set; }
    }
}
