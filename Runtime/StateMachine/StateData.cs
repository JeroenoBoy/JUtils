using System;

namespace JUtils
{
    /// <summary>
    /// The data of a state
    /// </summary>
    /// <example><code>
    /// namespace Example
    /// {
    ///     public class StateDataExample : State
    ///     {
    ///         private GameObject _target;
    /// 
    ///         public override void OnActivate()
    ///         {
    ///             _target = Data.Get&#60;GameObject>(0);
    ///             if (data.TryGet(1, out Transform optionalTransform))
    ///                 optionalTransform.position = Vector3.zero;
    ///         }
    /// 
    ///         public override void OnDeactivate()
    ///         {
    ///         }
    ///     }
    /// }
    /// </code></example>
    public class StateData
    {
        public readonly object[] arguments;


        /// <summary>
        /// Create a new StateData object with its arguments
        /// </summary>
        public StateData(params object[] arguments)
        {
            this.arguments = arguments;
        }


        /// <summary>
        /// Get an argument at an index
        /// </summary>
        public T Get<T>(int index)
        {
            if (!Has(index)) throw new ArgumentOutOfRangeException();

            object obj = arguments[index];
            if (obj is not T argument) throw new ArgumentException($"Type '{obj.GetType().Name}' is not castable to '{typeof(T).Name}'");
            return argument;
        }


        /// <summary>
        /// Try get the argument at an index
        /// </summary>
        public bool TryGet<T>(int index, out T argument)
        {
            argument = default;
            if (!Has(index)) return false;

            object obj = arguments[index];
            if (obj is not T obj1) return false;

            argument = obj1;
            return true;
        }


        /// <summary>
        /// See if the index exists
        /// </summary>
        public bool Has(int index)
        {
            return index < arguments.Length;
        }


        /// <summary>
        /// See if the index exists, and if its of the same type
        /// </summary>
        public bool Has<T>(int index)
        {
            return index < arguments.Length && arguments[index] is T;
        }
    }
}