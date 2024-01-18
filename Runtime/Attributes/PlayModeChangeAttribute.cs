using System;

namespace JUtils
{
    /// <summary>
    /// Receive a call when the play mode has been changed in a resource. Can be used to reset some data of a resource
    /// </summary>
    /// <example>
    /// [ResourcePlayModeChangeCallbackReceiver(resourcesFolder = "Events")]
    /// public partial class EventChannel : IResourcePlayModeChangeCallbackReceiver // You don't need to inherit from <see cref="IResourcePlayModeChangeCallbackReceiver"/>. It helps with getting the functions available
    /// {
    ///     // Must be public!
    ///     public void OnPlayModeEnter() { }
    ///
    /// 
    ///     // Must be public!
    ///     public void OnPlayModeExit() {
    ///         listeners = null;
    ///     }
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ResourcePlayModeChangeCallbackReceiverAttribute : Attribute
    {
        public readonly string folder;

        public ResourcePlayModeChangeCallbackReceiverAttribute(string resourcesFolder)
        {
            folder = resourcesFolder;
        }
    }


    /// <summary>
    /// This interface is optional but it gives all the methods for <see cref="ResourcePlayModeChangeCallbackReceiverAttribute"/>>
    /// </summary>
    public interface IResourcePlayModeChangeCallbackReceiver
    {
        public void OnPlayModeEnter();
        public void OnPlayModeExit();
    }
}