using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// Simple interface for working with singletons
    /// </summary>
    public interface ISingleton<T> where T : Object, ISingleton<T> { }
}
