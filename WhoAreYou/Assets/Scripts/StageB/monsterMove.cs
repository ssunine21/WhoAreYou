using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterMove : MonoBehaviour {

    Animator animator;
    public float speed;

    float movePower = 0.5f;

    bool set_idle = false;
    public static bool player_run = false;
    //public float movePower = 1.0f;

    //GameObject traceTarget;
    //Animator animator;
    //int movementFlag = 0; // 0: Idle, 1: Left, 2: Right
    //bool isTracing;

	// Use this for initialization
	void Start ()
    {
        animator = gameObject.GetComponent<Animator>();

        //animator = gameObject.GetComponentInChildren<Animator>();
        //traceTarget = gameObject.GetComponent<GameObject>();

        //StartCoroutine("ChangeMovement");
	}

    public void SetCall()
    {
        animator.SetBool("isCall", true);
    }

    public void SetRun()
    {
        animator.SetBool("isIdle", true);
        set_idle = true;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.right;
        if (set_idle)
        {
            if (movePower < speed)
                movePower += 0.3f;
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
    }
    //Coroutine
    //IEnumerator ChangeMovement()
    //{
    //    movementFlag = Random.Range(0, 4);

    //    if (movementFlag == 0 || movementFlag == 1)
    //        animator.SetBool("isRun", false);
    //    else
    //        animator.SetBool("isRun", true);

    //    yield return new WaitForSeconds(4.0f);

    //    StartCoroutine("ChangeMovement");
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayerBody" && !player_run)
        {
            animator.SetBool("isAttack", true);

            movePower = 0.0f;
            speed = 0.0f;

            other.gameObject.GetComponentInParent<PlayerController>().initSpeed = 0;
            other.gameObject.GetComponentInParent<PlayerController>().Dead();

            StartCoroutine("gameOver");
        }

        //if(other.gameObject.tag == "PlayerBody")
        //{
        //    traceTarget = other.gameObject;
        //    StopCoroutine("ChangeMovement");
        //}
    }
    IEnumerator gameOver()
    {
        yield return new WaitForSeconds(4.0f);

        GameOver.game_over = true;
    }
    
    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if(other.gameObject.tag == "PlayerBody")
    //    {
    //        isTracing = true;
    //        animator.SetBool("isRun", true);
    //    }
    //}

    //   void OnTriggerExit2D(Collider2D other)
    //   {
    //    if(other.gameObject.tag == "PlayerBody")
    //       {
    //           isTracing = false;
    //           StartCoroutine("ChangeMovement");
    //       }   
    //   }


    //    Update is called once per frame


    //void Move()
    //{
    //    Vector3 moveVelocity = Vector3.zero;
    //    string dist = "";

    //    if(isTracing)
    //    {
    //        Vector3 playerPos = traceTarget.transform.position;
    //        if (playerPos.x < transform.position.x)
    //            dist = "leftRun";
    //        else if (playerPos.x > transform.position.x)
    //            dist = "rightRun";
    //    }
    //    else
    //    {
    //        if (movementFlag == 0)
    //            dist = "leftWork";
    //        else if (movementFlag == 1)
    //            dist = "rightWork";
    //        else if (movementFlag == 2)
    //            dist = "leftRun";
    //        else if (movementFlag == 3)
    //            dist = "rightRun";
    //    }

    //    if (dist == "leftWork")
    //    {
    //        movePower = 2.0f;
    //        moveVelocity = Vector3.left;
    //        transform.localScale = new Vector3(-1, 1, 1);
    //    }
    //    else if (dist == "rightWork")
    //    {
    //        movePower = 2.0f;
    //        moveVelocity = Vector3.right;
    //        transform.localScale = new Vector3(1, 1, 1);
    //    }
    //    else if (dist == "leftRun")
    //    {
    //        movePower = 4.0f;
    //        moveVelocity = Vector3.left;
    //        transform.localScale = new Vector3(-1, 1, 1);
    //    }
    //    else if (dist == "rightRun")
    //    {
    //        movePower = 4.0f;
    //        moveVelocity = Vector3.right;
    //        transform.localScale = new Vector3(1, 1, 1);
    //    }

    //    transform.position += moveVelocity * movePower * Time.deltaTime;
    //}
}
