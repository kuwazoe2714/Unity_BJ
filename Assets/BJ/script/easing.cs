using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 「EasingFunction」というデリゲートを定義
public delegate float EasingFunction(float t, float b, float c, float d);
public delegate void EasingVector3(Vector3 value);

public abstract class EasingCoroutine : CustomYieldInstruction
{
    public float startTime;
    public float duration;
    // func変数にメソッドを入れて初めてdelegateとして機能する
    public EasingFunction func;

    protected abstract void Update(float t);

    public override bool keepWaiting
    {
        get
        {
            var elapsed = (Time.time - startTime);
            var t = func(elapsed, 0f, 1f, duration);
            Update(t);
            return (elapsed < duration);
        }
    }
}

public class EasingVector3Coroutine : EasingCoroutine
{
    public Vector3 from;
    public Vector3 to;
    public EasingVector3 updateFunc;

    protected override void Update(float t)
    {
        updateFunc(Vector3.Lerp(from, to, t));
    }
}

public static class easing
{
    public static EasingCoroutine CreateVector3(EasingFunction func, Vector3 from, Vector3 to, float duration, EasingVector3 update)
    {
        return new EasingVector3Coroutine
        {
            from = from,
            to = to,
            duration = duration,
            startTime = Time.time,
            func = func,
            updateFunc = update,
        };
    }

    public static float easeQuad(float t, float b, float c, float d)
    {
        t /= d;
        return c * t + b;
    }

    public static float easeInQuad(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t + b;
    }

    public static float easeOutQuad(float t, float b, float c, float d)
    {
        t /= d;
        return -c * t * (t - 2) + b;
    }

}
