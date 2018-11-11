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
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            cameraPosition.x = player.transform.position.x - 3.0f;
            cameraPosition.y = player.transform.position.y + 2.0f;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            cameraPosition.x = player.transform.position.x + 3.0f;
            cameraPosition.y = player.transform.position.y + 2.0f;
        }

        if (cameraPosition.x < 0)
            cameraPosition.x = 0.0f;
        else if (cameraPosition.x > 43)
            cameraPosition.x = 43.0f;

        if (cameraPosition.y < 0)
            cameraPosition.y = 0.0f;
        else if (cameraPosition.y > 10.4f)
            cameraPosition.y = 10.4f;

        cameraPosition.z = -10;

        transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
    }

    // Use this for initialization
    void Start ()
    {
        cameraPosition.x = player.transform.position.x + 3.0f;
        cameraPosition.y = player.transform.position.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
