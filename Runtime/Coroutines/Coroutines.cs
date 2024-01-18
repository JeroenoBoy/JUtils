using System;
using System.Collections;
using JUtils.Internal;
using UnityEngine;



namespace JUtils
{
    /// <summary>
    /// Make coroutines run on the <see cref="JUtilsObject"/>
    /// </summary>
    /// <example><code lang="CSharp">
    /// Coroutines.RunDelay(2f, () => Debug.Log("Delay triggered"))
    /// </code></example>
    public static class Coroutines
    {
        /// <summary>
        /// Run a action in the next frame
        /// </summary>
        public static Coroutine RunNextFrame(Action action)
        {
            return JUtilsObject.instance.StartCoroutine(Routines.NextFrameRoutine(action));
        }


        /// <summary>
        /// Run a action after a certain amount of seconds;
        /// -- You should always use StartCoroutine instead of the Coroutines, but this can be more useful.
        /// </summary>
        public static Coroutine RunDelay(float delay, Action action)
        {
            return JUtilsObject.instance.StartCoroutine(Routines.DelayRoutine(delay, action));
        }


        /// <summary>
        /// Run a action after a certain amount of seconds
        /// -- You should always use StartCoroutine instead of the Coroutines, but this can be more useful.
        /// </summary>
        public static Coroutine RunDelay(TimeSpan delay, Action action)
        {
            return JUtilsObject.instance.StartCoroutine(Routines.DelayRoutine(delay, action));
        }


        /// <summary>
        /// Run a routine on the JUtils Object
        /// -- You should always use StartCoroutine instead of the Coroutines, but this can be more useful.
        /// </summary>
        public static Coroutine Run(IEnumerator routine)
        {
            return JUtilsObject.instance.StartCoroutine(routine);
        }
    }
}
