using System;
using System.Collections;

namespace JUtils
{
    /// <summary>
    /// Wrapper for troublesome coroutines
    /// </summary>
    /// <example><code lang="CSharp">
    /// private IEnumerator EnterShipRoutine()
    /// {
    ///     playerMovement.Freeze();
    /// 
    ///     CoroutineCatcher catcher = Routines.Catcher(MoveIntoShip());
    ///     yield return cather; // Runs the TroublesomeRoutine
    ///
    ///     if (catcher.HasThrown(out Exception exception)) {
    ///         Debug.LogException(catcher.exception);
    ///     }
    ///
    ///     playerMovement.UnFreeze();
    /// }
    /// </code></example>
    public class CoroutineCatcher : IEnumerator
    {
        private readonly IEnumerator _enumerator;
        private Exception _threwException;
        
        
        /// <summary>
        /// This allows you to catch errors in enumerators
        /// </summary>
        public CoroutineCatcher(IEnumerator coroutine)
        {
            _enumerator = coroutine;
        }


        /// <summary>
        /// Check if the coroutine has thrown an exception
        /// </summary>
        public bool HasThrown(out Exception exception)
        {
            exception = _threwException;
            return exception != null;
        }


        /// <summary>
        /// Move to the next element in the enumerator
        /// </summary>
        public bool MoveNext()
        {
            if (_threwException != null) return false;
            bool canContinue = false;
            
            try {
                canContinue = _enumerator.MoveNext();
            }
            catch(Exception e) {
                _threwException = e;
            }
            
            return _threwException == null && canContinue;
        }


        /// <summary>
        /// Reset the enumerator
        /// </summary>
        public void Reset()
        {
            _enumerator.Reset();
        }


        /// <summary>
        /// Get the current value of the enumerator
        /// </summary>
        public object Current => _enumerator.Current;
    }
}