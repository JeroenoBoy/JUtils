using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace JUtils
{
    /// <summary>
    /// A static class that contains methods for easily creating routines & avoid object allocations
    /// </summary>
    public static class Routines
    {
        private static Dictionary<float, WaitForSeconds> _wfsDictionary;
        public static  Dictionary<float, WaitForSeconds> wfsDictionary => _wfsDictionary ??= new Dictionary<float, WaitForSeconds>();

        
        private static Dictionary<float, WaitForSecondsRealtime> _wfsRtDictionary;
        public static Dictionary<float, WaitForSecondsRealtime> wfsRtDictionary => _wfsRtDictionary ??= new Dictionary<float, WaitForSecondsRealtime>();

        
        private static WaitForFixedUpdate _waitForFixedUpdate;
        private static WaitForEndOfFrame _waitForEndOfFrame;
        

        /// <summary>
        /// Faster alternative to WaitForSeconds, caches the object.
        /// This will permanently cache the instance unless removed with Coroutines.wfsDictionary
        /// </summary>
        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            Dictionary<float, WaitForSeconds> dict = wfsDictionary;
            if (wfsDictionary.ContainsKey(seconds)) return dict[seconds];
            return dict[seconds] = new WaitForSeconds(seconds);
        }
        

        /// <summary>
        /// Faster alternative to creating a new WaitForSecondsRealtime, caches the object.
        /// This will permanently cache the instance unless removed with Coroutines.wfsRtDictionary
        /// </summary>
        public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
        {
            Dictionary<float, WaitForSecondsRealtime> dict = wfsRtDictionary;
            if (wfsDictionary.ContainsKey(seconds)) return dict[seconds];
            return dict[seconds] = new WaitForSecondsRealtime(seconds);
        }

        
        /// <summary>
        /// Return the cached WaitForFixedUpdate instance
        /// </summary>
        public static WaitForFixedUpdate WaitForFixedUpdate() => _waitForFixedUpdate ??= new WaitForFixedUpdate();

        
        /// <summary>
        /// Return the cached WaitForFixedUpdate instance
        /// </summary>
        public static WaitForEndOfFrame WaitForEndOfFrame() => _waitForEndOfFrame ??= new WaitForEndOfFrame();


        /// <summary>
        /// Returns a CoroutineCatcher
        /// </summary>
        public static CoroutineCatcher Catcher(IEnumerator coroutine) => new (coroutine);


        /// <summary>
        /// A routine that runs the action in the next frame
        /// </summary>
        public static IEnumerator NextFrameRoutine(Action action)
        {
            yield return null;
            action.Invoke();
        }


        /// <summary>
        /// A routine that runs the action at the given delay
        /// </summary>
        public static IEnumerator DelayRoutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action.Invoke();
        }
        

        /// <summary>
        /// A routine that runs the action after the given timespan
        /// </summary>
        public static IEnumerator DelayRoutine(TimeSpan timeSpan, Action action)
        {
            yield return new WaitForSeconds(timeSpan.Seconds);
            action.Invoke();
        }
    }
    
    
    
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
