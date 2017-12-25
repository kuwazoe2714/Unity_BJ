using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_easing : MonoBehaviour {

    public float b = 0;
    public float c = 1;
    public float d;

    public Vector3 from;
    public Vector3 to;

    private float startTime;

    // Use this for initialization
    void Start ()
    {
        startTime = Time.time;
        from = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float t = Time.time;

        if (t <= d)
        {
            t -= startTime;
            float e = easing.easeOutQuad(t, b, c, d);
            
            var inter = to - from;
            inter *= e;
            transform.localPosition = inter;
        }

    }
}
