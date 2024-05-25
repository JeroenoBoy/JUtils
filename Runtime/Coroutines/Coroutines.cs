using System;
using System.Collections;
using JetBrains.Annotations;
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
        /// Run an action in the next frame
        /// </summary>
        public static Coroutine RunNextFrame([NotNull] Action action)
        {
            return JUtilsObject.instance.StartCoroutine(Routines.NextFrameRoutine(action));
        }

        /// <summary>
        /// Run an action after a certain amount of seconds;
        /// -- You should always use StartCoroutine instead of the Coroutines, but this can be more useful.
        /// </summary>
        public static Coroutine RunDelay(float delay, [NotNull] Action action)
        {
            return JUtilsObject.instance.StartCoroutine(Routines.DelayRoutine(delay, action));
        }

        /// <summary>
        /// Run an action after a certain amount of seconds
        /// -- You should always use StartCoroutine instead of the Coroutines, but this can be more useful.
        /// </summary>
        public static Coroutine RunDelay(TimeSpan delay, [NotNull] Action action)
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

        /// <summary>
        /// Allows method chaining for coroutines
        /// </summary>
        public static Coroutine Then(this Coroutine coroutine, [NotNull] Action action)
        {
            return JUtilsObject.instance.StartCoroutine(Routines.ThenRoutine(coroutine, action));
        }
    }
}