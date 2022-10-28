using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace JUtils.Extensions
{
    public static class Coroutines
    {
        private static Dictionary<int, WaitForSeconds> _wfsDictionary;
        public static Dictionary<int, WaitForSeconds> wfsDictionary => _wfsDictionary ??= new Dictionary<int, WaitForSeconds>();

        
        private static Dictionary<int, WaitForSecondsRealtime> _wfsRtDictionary;
        public static Dictionary<int, WaitForSecondsRealtime> wfsRtDictionary => _wfsRtDictionary ??= new Dictionary<int, WaitForSecondsRealtime>();

        
        private static WaitForFixedUpdate _waitForFixedUpdate;
        private static WaitForEndOfFrame _waitForEndOfFrame;
        

        /// <summary>
        /// Faster alternative to WaitForSeconds, caches the object.
        /// This will permanently cache the instance unless removed with Coroutines.wfsDictionary
        /// </summary>
        /// <param name="seconds">The amount of seconds to wait</param>
        /// <returns></returns>
        public static WaitForSeconds WaitForSeconds(int seconds)
        {
            Dictionary<int, WaitForSeconds> dict = wfsDictionary;
            if (wfsDictionary.ContainsKey(seconds)) return dict[seconds];
            return dict[seconds] = new WaitForSeconds(seconds);
        }
        

        /// <summary>
        /// Faster alternative to creating a new WaitForSecondsRealtime, caches the object.
        /// This will permanently cache the instance unless removed with Coroutines.wfsRtDictionary
        /// </summary>
        /// <param name="seconds">The amount of seconds to wait</param>
        /// <returns></returns>
        public static WaitForSecondsRealtime WaitForSecondsRealtime(int seconds)
        {
            Dictionary<int, WaitForSecondsRealtime> dict = wfsRtDictionary;
            if (wfsDictionary.ContainsKey(seconds)) return dict[seconds];
            return dict[seconds] = new WaitForSecondsRealtime(seconds);
        }

        
        /// <summary>
        /// Return the cached WaitForFixedUpdate instance
        /// </summary>
        /// <returns></returns>
        public static WaitForFixedUpdate WaitForFixedUpdate() => _waitForFixedUpdate ??= new WaitForFixedUpdate();

        
        /// <summary>
        /// Return the cached WaitForFixedUpdate instance
        /// </summary>
        /// <returns></returns>
        public static WaitForEndOfFrame WaitForEndOfFrame() => _waitForEndOfFrame ??= new WaitForEndOfFrame();


        /// <summary>
        /// Returns a CoroutineCatcher
        /// </summary>
        /// <returns></returns>
        public static CoroutineCatcher Catcher(IEnumerator coroutine) => new (coroutine);
    }
    
    
    
    public class CoroutineCatcher : IEnumerator
    {
        private readonly IEnumerator _enumerator;
        private Exception _threwException;
        
        
        /// <summary>
        /// This allows you to catch errors in enumerators
        /// </summary>
        /// <param name="coroutine"></param>
        public CoroutineCatcher(IEnumerator coroutine)
        {
            _enumerator = coroutine;
        }


        /// <summary>
        /// Check if the coroutine has thrown an exception
        /// </summary>
        /// <param name="exception">The caught exception, can be null</param>
        /// <returns>True if there was an exception</returns>
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
