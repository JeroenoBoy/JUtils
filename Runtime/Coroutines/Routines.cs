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
}
