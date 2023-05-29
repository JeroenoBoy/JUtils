using System;



namespace JUtils.FSM
{
    public class StateData
    {
        public object[] Arguments;
        
        
        public StateData(params object[] arguments)
        {
            Arguments = arguments;
        }
        
       
        /// <summary>
        /// Get an argument at an index
        /// </summary>
        public T Get<T>(int index)
        {
            if (!Has(index))
                throw new ArgumentOutOfRangeException();

            object obj = Arguments[index];

            if (obj is not T argument)
                throw new ArgumentException($"Type '{obj.GetType().Name}' is not castable to '{typeof(T).Name}'");
            
            return argument;
        }


        /// <summary>
        /// Try get the argument at an index
        /// </summary>
        public bool TryGet<T>(int index, out T argument)
        {
            argument = default;
            if (!Has(index)) return false;
            
            object obj = Arguments[index];
            if (obj is not T obj1) return false;
            
            argument = obj1;
            return true;
        }


        /// <summary>
        /// See if the index exists
        /// </summary>
        public bool Has(int index)
        {
            return index < Arguments.Length;
        }

        
        /// <summary>
        /// See if the index exists, and if its of the same type
        /// </summary>
        public bool Has<T>(int index)
        {
            return index < Arguments.Length && Arguments[index] is T;
        }
    }
}
