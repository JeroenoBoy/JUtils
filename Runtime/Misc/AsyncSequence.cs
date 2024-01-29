using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUtils
{
    /// <summary>
    /// A class for creating simple tweens
    /// </summary>
    public static class AsyncSequence
    {
        /// <summary>
        /// Runs a given action when a task has been completed
        /// </summary>
        public static async Task Then(this Task task, [NotNull] Action action)
        {
            await task;
            action();
        }


        public static async Task Delay(float delay, CancellationToken cancellationToken)
        {
            if (delay <= 0) return;
            await Awaitable.WaitForSecondsAsync(delay, cancellationToken);
        }


        public static async Task Delay(this Task task, float delay, CancellationToken cancellationToken)
        {
            await task;
            if (delay >= 0) await Awaitable.WaitForSecondsAsync(delay, cancellationToken);
        }


        public static Task Tween(Vector3 a, Vector3 b, float duration, JEase ease, CancellationToken cancellationToken, Action<Vector3> setter)
        {
            return Tween(0, 1, duration, ease, cancellationToken, t => setter(Vector3.Lerp(a, b, t)));
        }


        public static Task Tween(Translate a, Translate b, float duration, JEase ease, CancellationToken cancellationToken, Action<Translate> setter)
        {
            LengthUnit unitX = a.x.unit;
            LengthUnit unitY = a.y.unit;

            if (unitX != b.x.unit) throw new Exception("Unit of a.x does not match b.x");
            if (unitY != b.y.unit) throw new Exception("Unit of a.x does not match b.x");

            return Tween(
                0, 1, duration, ease, cancellationToken,
                t => setter(
                    new Translate(
                        new Length(Mathf.Lerp(a.x.value, b.x.value, t), unitX),
                        new Length(Mathf.Lerp(a.y.value, b.y.value, t), unitX),
                        Mathf.Lerp(a.z, b.z, t)
                    )
                )
            );
        }


        public static Task Tween(Vector2 a, Vector2 b, float duration, JEase ease, CancellationToken cancellationToken, Action<Vector2> setter)
        {
            return Tween(0, 1, duration, ease, cancellationToken, t => setter(Vector2.Lerp(a, b, t)));
        }


        public static async Task Tween(float a, float b, float duration, JEase ease, CancellationToken cancellationToken, Action<float> setter)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / duration) {
                setter(ease.Lerp(a, b, t));
                await Awaitable.NextFrameAsync(cancellationToken);
            }

            setter(1);
        }


        public static async Task Tween(this Task task, Vector3 a, Vector3 b, float duration, JEase ease, CancellationToken cancellationToken, Action<Vector3> setter)
        {
            await task;
            await Tween(a, b, duration, ease, cancellationToken, setter);
        }


        public static async Task Tween(this Task task, Translate a, Translate b, float duration, JEase ease, CancellationToken cancellationToken, Action<Translate> setter)
        {
            await task;
            await Tween(a, b, duration, ease, cancellationToken, setter);
        }


        public static async Task Tween(this Task task, Vector2 a, Vector2 b, float duration, JEase ease, CancellationToken cancellationToken, Action<Vector2> setter)
        {
            await task;
            await Tween(a, b, duration, ease, cancellationToken, setter);
        }


        public static async Task Tween(this Task task, float a, float b, float duration, JEase ease, CancellationToken cancellationToken, Action<float> setter)
        {
            await task;
            await Tween(a, b, duration, ease, cancellationToken, setter);
        }
    }
}