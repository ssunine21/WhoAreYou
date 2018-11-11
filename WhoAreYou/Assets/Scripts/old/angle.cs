using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angle : MonoBehaviour {

    public float movePower = 500.0f;

    Rigidbody2D rigid;
    
    bool isJumping;


    // Use this for initialization
    void Start()
    {
        isJumping = false;
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            moveVelocity = Vector3.right;
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            moveVelocity = Vector3.left;
        }


        transform.Rotate(moveVelocity * movePower * Time.deltaTime * 80f);
    }
}
