using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasingMove : MonoBehaviour {

    private float b = 0;
    private float c = 1;

    private static float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    /**
     * <summary> アニメーション曲線関数 </summary>
     */
    public static Vector3 CardMove(Vector3 from, Vector3 to, int finishTime, bool finishAnimation)
    {
        var t = Time.time;

        if (t <= finishTime)
        {
            t -= startTime;
            float e = easing.easeInQuad(t, 0, 1, finishTime);

            var inter = to - from;
            inter *= e;
            return inter;
        }
        finishAnimation = true;
        return new Vector3(0,0);
    }
}
