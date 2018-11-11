using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_NonPhysics2D : MonoBehaviour
{

    public float movePower = 1.0f;
    public float jumpPower = 1.0f;

    Rigidbody2D rigid;

    Renderer Render;
    Animator animator;
    Animator other_animator;
    

    bool isJumping = false;
    bool action = false;


    // Use this for initialization
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        Render = gameObject.GetComponent<Renderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 움직임을 체크해서 애니매이션 바꾸기 Move() 함수에 true값 있음

        if (Input.GetAxisRaw ("Horizontal") == 0)
            animator.SetBool("isMoving", false);

        if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("isJumping"))
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
            animator.SetTrigger("doJumping");
        }

        // 액션
        if (Input.GetKeyDown(KeyCode.X))
            action = true;


        // 계단 안쪽으로 갈 때 계단에 캐릭터 가리기
        if (transform.position.y > 3.0f && transform.position.y < 7.5f)
        {
            this.Render.sortingLayerName = "Building";
            this.Render.sortingOrder = -2;
        }
        else
        {
            this.Render.sortingLayerName = "Char";
            this.Render.sortingOrder = 0;
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;

            this.transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("isMoving", true);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;

            this.transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("isMoving", true);
        }

        this.transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!isJumping)
            return;

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("stairs") && (other.transform.position.y + 1.3f) < this.transform.position.y)
            other.isTrigger = false;

        if (this.rigid.velocity.y < 0.5f) // other.gameObject.layer == 0
            animator.SetBool("isJumping", false);

        other_animator = other.gameObject.GetComponentInChildren<Animator>();

        if (other.CompareTag("main_switch") && action == true)
        {
            other_animator.SetBool("isSwitch", true);
        }

        action = false;
        
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Detach : " + other.gameObject.layer);

        if (other.CompareTag("stairs"))
            other.isTrigger = true;
    }


    void Interation()
    {

    }
}


