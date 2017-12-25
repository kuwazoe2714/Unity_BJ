using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class easingmove : AnimationManager {


    private float b = 0;
    private float c = 1;

    private float startTime;
    private float d;

    private Vector3 from;
    private Vector3 to;

    public GameObject Card_Obj { get; set; }
    private bool isInit;

    private void Start()
    {
        startTime = Time.time;
        isInit = false;
    }

    /**
     * <summary> アニメーション曲線関数 </summary>
     */
    public Vector3 CardMove(Vector3 frompos, Vector3 topos, float finishTime, bool finishAnimation)
    {
        from = frompos;
        if (!isInit)
        {
            to = topos;
            d = finishTime;
            isInit = true;
        }
        var t = Time.time;

        if (t <= finishTime)
        {
            t -= startTime;
            float e = easing.easeInQuad(t, 0, 1, finishTime);

            var inter = to - from;
            inter *= e;
            return inter;
        }
        EndCall();
        return new Vector3(0, 0);
    }

    private void Update()
    {
        Debug.Log("カード動く");
        Card_Obj.transform.position += CardMove(from, to, d, false);
        Debug.Log(Card_Obj.transform.position);
        from = Card_Obj.transform.position;
    }

    public override void EndCall()
    {
        Debug.Log("アニメーション曲線完了！");
    }

}
