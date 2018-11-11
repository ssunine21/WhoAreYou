using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pierrot : MonoBehaviour {
    

    Vector3 v3;
    

    // Update is called once per frame

    public void StageAShow()
    {
        v3.x = 14.0f; v3.y = -61.0f; v3.z = 0.0f;
        this.transform.position = v3;
    }
}
