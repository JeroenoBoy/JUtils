using System;
using System.Collections;
using UnityEngine;



namespace JUtils
{
    public static class Routines
    {
        public static IEnumerator NextFrameRoutine(Action action)
        {
            yield return null;
            action.Invoke();
        }


        public static IEnumerator DelayRoutine(float timeSpan, Action action)
        {
            yield return new WaitForSeconds(timeSpan);
            action.Invoke();
        }
        

        public static IEnumerator DelayRoutine(TimeSpan timeSpan, Action action)
        {
            yield return new WaitForSeconds(timeSpan.Seconds);
            action.Invoke();
        }
    }
}
