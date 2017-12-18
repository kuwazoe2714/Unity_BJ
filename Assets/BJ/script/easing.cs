using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class easing
{
    public static float easeInQuad(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t + b;
        //    100 * t * t* -100
    }

    public static float easeOutQuad(float t, float b, float c, float d)
    {
        t /= d;
        return -c * t * (t - 2) + b;
    }

}
