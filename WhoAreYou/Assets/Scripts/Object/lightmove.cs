using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightmove : MonoBehaviour {

    public float originalRange;
    public float duration = 3.0f;
    public Light lt;

    // Use this for initialization
    void Start ()
    {
        lt = GetComponent<Light>();
        originalRange = lt.range;
    }
	
	// Update is called once per frame
	void Update () {
        float amplitude = Mathf.PingPong(Time.time, duration);
        amplitude = amplitude / duration * 0.9f + 0.9f;
        lt.range = originalRange * amplitude;
	}
}
