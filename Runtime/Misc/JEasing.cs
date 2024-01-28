using UnityEngine;

namespace JUtils
{
    /// <summary>
    /// A collection of common easing functions
    /// </summary>
    public enum JEase
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack,
        InElastic,
        OutElastic,
        InOutElastic,
        InBounce,
        OutBounce,
        InOutBounce
    }


    /// <summary>
    /// The implementation of common easing functions.
    /// </summary>
    public static class JEasing
    {
        public static float Evaluate(this JEase jEase, float time)
        {
            time = Mathf.Clamp01(time);
            return jEase switch {
                JEase.Linear => time,
                JEase.InSine => EaseInSine(time),
                JEase.OutSine => EaseOutSine(time),
                JEase.InOutSine => EaseInOutSine(time),
                JEase.InQuad => EaseInQuad(time),
                JEase.OutQuad => EaseOutQuad(time),
                JEase.InOutQuad => EaseInOutQuad(time),
                JEase.InCubic => EaseInCubic(time),
                JEase.OutCubic => EaseOutCubic(time),
                JEase.InOutCubic => EaseInOutCubic(time),
                JEase.InQuart => EaseInQuart(time),
                JEase.OutQuart => EaseOutQuart(time),
                JEase.InOutQuart => EaseInOutQuart(time),
                JEase.InQuint => EaseInQuint(time),
                JEase.OutQuint => EaseOutQuint(time),
                JEase.InOutQuint => EaseInOutQuint(time),
                JEase.InExpo => EaseInExpo(time),
                JEase.OutExpo => EaseOutExpo(time),
                JEase.InOutExpo => EaseInOutExpo(time),
                JEase.InCirc => EaseInCirc(time),
                JEase.OutCirc => EaseOutCirc(time),
                JEase.InOutCirc => EaseInOutCirc(time),
                JEase.InBack => EaseInBack(time),
                JEase.OutBack => EaseOutBack(time),
                JEase.InOutBack => EaseInOutBack(time),
                JEase.InElastic => EaseInElastic(time),
                JEase.OutElastic => EaseOutElastic(time),
                JEase.InOutElastic => EaseInOutElastic(time),
                JEase.InBounce => EaseInBounce(time),
                JEase.OutBounce => EaseOutBounce(time),
                JEase.InOutBounce => EaseInOutBounce(time),
                _ => 0
            };
        }


        public static float Evaluate(this JEase jEase, float time, float duration)
        {
            return jEase.Evaluate(time / duration);
        }


        public static float Evaluate(this JEase jEase, float time, float duration, float delay)
        {
            return jEase.Evaluate((time - delay) / duration);
        }


        public static float Lerp(this JEase jEase, float a, float b, float time)
        {
            return Mathf.Lerp(a, b, jEase.Evaluate(time));
        }


        public static float Linear(float time)
        {
            return time;
        }


        public static float EaseInSine(float time)
        {
            return 1f - Mathf.Cos(time * Mathf.PI * 0.5f);
        }


        public static float EaseOutSine(float time)
        {
            return Mathf.Sin(time * Mathf.PI * .5f);
        }


        public static float EaseInOutSine(float time)
        {
            return -.5f * (Mathf.Cos(Mathf.PI * time) - 1);
        }


        public static float EaseInQuad(float time)
        {
            return time * time;
        }


        public static float EaseOutQuad(float time)
        {
            return 1 - (1 - time) * (1 - time);
        }


        public static float EaseInOutQuad(float time)
        {
            return time < .5f ? 2 * time * time : 1 - Mathf.Pow(-2 * time + 2, 2) * .5f;
        }


        public static float EaseInCubic(float time)
        {
            return time * time * time;
        }


        public static float EaseOutCubic(float time)
        {
            return 1 - Mathf.Pow(1 - time, 3);
        }


        public static float EaseInOutCubic(float time)
        {
            return time < .5f ? 4 * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 3) * .5f;
        }


        public static float EaseInQuart(float time)
        {
            return time * time * time * time;
        }


        public static float EaseOutQuart(float time)
        {
            return 1 - Mathf.Pow(1 - time, 4);
        }


        public static float EaseInOutQuart(float time)
        {
            return time < .5f ? 8 * time * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 4) * .5f;
        }


        public static float EaseInQuint(float time)
        {
            return time * time * time * time * time;
        }


        public static float EaseOutQuint(float time)
        {
            return 1 - Mathf.Pow(1 - time, 5);
        }


        public static float EaseInOutQuint(float time)
        {
            return time < .5f ? 16 * time * time * time * time * time : 1 - Mathf.Pow(-2 * time + 2, 5) * .5f;
        }


        public static float EaseInExpo(float time)
        {
            return time == 0 ? 0 : Mathf.Pow(2, 10 * (time - 10));
        }


        public static float EaseOutExpo(float time)
        {
            return time == 1 ? 1 : 1 - Mathf.Pow(2, -10 * time);
        }


        public static float EaseInOutExpo(float time)
        {
            return time switch {
                0 => 0,
                1 => 1,
                < .5f => Mathf.Pow(2, 20 * time - 10) * .5f,
                _ => (2 - Mathf.Pow(2, -20 * time + 10)) * .5f
            };
        }


        public static float EaseInCirc(float time)
        {
            return 1 - Mathf.Sqrt(1 - Mathf.Pow(time, 2));
        }


        public static float EaseOutCirc(float time)
        {
            return Mathf.Sqrt(1 - Mathf.Pow(time - 1, 2));
        }


        public static float EaseInOutCirc(float time)
        {
            return time < .5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * time, 2))) * .5f : (Mathf.Sqrt(1 - Mathf.Pow(-2 * time + 2, 2)) + 1) * .5f;
        }


        public static float EaseInBack(float time)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 + 1;
            return c2 * time * time * time - c1 * time * time;
        }


        public static float EaseOutBack(float time)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 + 1;
            return 1 + c2 * Mathf.Pow(time - 1, 3) + c1 * Mathf.Pow(time - 1, 2);
        }


        public static float EaseInOutBack(float time)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 + 1;
            return time < .5f
                ? Mathf.Pow(2 * time, 2) * ((c2 + 1) * 2 * time - c2) * .5f
                : (Mathf.Pow(2 * time - 2, 2) * ((c2 + 1) * (time * 2 - 2) + c2) + 2) * .5f;
        }


        public static float EaseInElastic(float time)
        {
            const float c4 = 2 * Mathf.PI / 3;
            return time switch {
                0 => 0,
                1 => 1,
                _ => -1 * Mathf.Pow(2, 10 * (time - 1)) * Mathf.Sin((time - 1.1f) * c4)
            };
        }


        public static float EaseOutElastic(float time)
        {
            const float c4 = 2 * Mathf.PI / 3;
            return time switch {
                0 => 0,
                1 => 1,
                _ => Mathf.Pow(2, -10 * time) * Mathf.Sin((time * 10 - .75f) * c4) + 1
            };
        }


        public static float EaseInOutElastic(float time)
        {
            const float c5 = 2 * Mathf.PI / 4.5f;
            return time switch {
                0 => 0,
                1 => 1,
                < .5f => -(Mathf.Pow(2, 20 * time - 10) * Mathf.Sin((20 * time - 11.125f) * c5)) * .5f,
                _ => Mathf.Pow(2, -20 * time + 10) * Mathf.Sin((20 * time - 11.125f) * c5) * .5f + 1
            };
        }


        public static float EaseInBounce(float time)
        {
            return 1 - EaseOutBounce(1 - time);
        }


        public static float EaseOutBounce(float time)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            return time switch {
                < 1 / d1 => n1 * time * time,
                < 2 / d1 => n1 * (time -= 1.5f / d1) * time + .75f,
                < 2.5f / d1 => n1 * (time -= 2.25f / d1) * time + .9375f,
                _ => n1 * (time -= 2.625f / d1) * time + .984375f
            };
        }


        public static float EaseInOutBounce(float time)
        {
            return time < .5f
                ? (1 - EaseOutBounce(1 - 2 * time)) * .5f
                : (1 + EaseOutBounce(2 * time - 1)) * .5f;
        }
    }
}