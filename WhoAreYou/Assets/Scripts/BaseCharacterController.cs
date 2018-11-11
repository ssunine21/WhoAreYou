using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour {

    // 외부 파라미터
    public Vector2 velocityMin = new Vector2(-100.0f, -100.0f);
    public Vector2 velocityMax = new Vector2(100.0f, 50.0f);

    [System.NonSerialized] public float dir = 1.0f;
    [System.NonSerialized] public float speed = 6.0f;
    [System.NonSerialized] public float basScaleX = 1.0f;
    [System.NonSerialized] public bool activeSts = false;
    [System.NonSerialized] public bool jumped = false;
    [System.NonSerialized] public bool grounded = false;
    [System.NonSerialized] public bool groundedPrev = false;


    // 캐쉬
    [System.NonSerialized] public Animator animator;
    [System.NonSerialized] public Rigidbody2D rigid;
    protected Transform groundCheck_L;
    protected Transform groundCheck_C;
    protected Transform groundCheck_R;

    // 내부 파라미터
    protected float speedVx = 0.0f;
    protected float speedVxAddPower = 0.0f;
    protected float gravityScale = 5.0f;
    protected float jumpStartTime = 0.0f;

    protected GameObject groundCheck_OnRoadObject;
    protected GameObject groundCheck_OnMoveObject;
    protected GameObject groundCheck_OnEnemyObject;

    // Use this for initialization
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        groundCheck_L = transform.Find("GroundCheck_L");
        groundCheck_C = transform.Find("GroundCheck_C");
        groundCheck_R = transform.Find("GroundCheck_R");

        dir = (transform.localScale.x > 0.0f) ? 1 : -1;
        basScaleX = transform.localScale.x * dir;
        transform.localScale = new Vector3(basScaleX, transform.localScale.y, transform.localScale.z);

        activeSts = true;
        gravityScale = rigid.gravityScale;
    }
    
    protected virtual void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    protected virtual void FixedUpdate()
    {
        // 지면 체크
        groundedPrev = grounded;
        grounded = false;

        groundCheck_OnRoadObject = null;
        groundCheck_OnMoveObject = null;
        groundCheck_OnEnemyObject = null;

        Collider2D[][] groundCheckCollider = new Collider2D[3][];
        groundCheckCollider[0] = Physics2D.OverlapPointAll(groundCheck_L.position);
        groundCheckCollider[1] = Physics2D.OverlapPointAll(groundCheck_C.position);
        groundCheckCollider[2] = Physics2D.OverlapPointAll(groundCheck_R.position);

        foreach (Collider2D[] groundCheckList in groundCheckCollider)
        {
            foreach (Collider2D groundCheck in groundCheckList)
            {
                if (groundCheck != null)
                {
                    if (!groundCheck.isTrigger)
                    {
                        grounded = true;
                        if (groundCheck.tag == "Road")
                            groundCheck_OnRoadObject = groundCheck.gameObject;

                        else if (groundCheck.tag == "MoveObject")
                            groundCheck_OnMoveObject = groundCheck.gameObject;

                        else if (groundCheck.tag == "Enemy")
                            groundCheck_OnEnemyObject = groundCheck.gameObject;
                    }
                }
            }
        }

        // 캐릭터 개별 처리
        FixedUpdateCharacter();

        // 이동 계산
        rigid.velocity = new Vector2(speedVx, rigid.velocity.y);

        // Velocity 값 체크
        float vx = Mathf.Clamp(rigid.velocity.x, velocityMin.x, velocityMax.x);
        float vy = Mathf.Clamp(rigid.velocity.y, velocityMin.y, velocityMax.y);
        rigid.velocity = new Vector2(vx, vy);
    }
    

    protected virtual void FixedUpdateCharacter()
    {
    }

    public virtual void ActionMove(float n)
    {
        if (n != 0.0f)
        {
            dir = Mathf.Sign(n);
            speedVx = speed * n;
            animator.SetTrigger("Run");
        }
        else
        {
            speedVx = 0;
            animator.SetTrigger("Idle");
        }
    }

    public virtual void Dead()
    {
        if (dir == 1)
            animator.SetBool("right_dead", true);
        else
            animator.SetBool("left_dead", true);
    }
}
