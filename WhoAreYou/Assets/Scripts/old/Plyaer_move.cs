using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plyaer_move : MonoBehaviour
{
    Rigidbody2D Rigid2D;

    public float JumpForce = 600.0f;
    public float speed = 0.5f;

	// Use this for initialization
	void Start ()
    {
        this.Rigid2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            this.transform.Translate(speed * Time.deltaTime, 0, 0);

        if (Input.GetKey(KeyCode.LeftArrow))
            this.transform.Translate(-speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
            this.Rigid2D.AddForce(transform.up * this.JumpForce);
	}
}
