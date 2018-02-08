using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera_Action : MonoBehaviour
{
    public GameObject player;
    public float followSpeed = 0;

    Vector3 cameraPosition;

    void LateUpdate()
    {
        cameraPosition.x = player.transform.position.x;
        if (cameraPosition.x < 0)
            cameraPosition.x = 0.0f;

        cameraPosition.y = player.transform.position.y;
        if (cameraPosition.y < 0)
            cameraPosition.y = 0.0f;
        cameraPosition.z = -10;

        transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
