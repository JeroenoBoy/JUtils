using UnityEngine;



namespace JUtils.Singletons
{
    /// <summary>
    /// Simple interface for working with singletons
    /// </summary>
    public interface ISingleton<T>
        where T : Object, ISingleton<T>
    {
        public static T instance;
    }
}
