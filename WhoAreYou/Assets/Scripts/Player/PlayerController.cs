using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseCharacterController {

    // 외부 파라미터
    [Range(0.1f, 100.0f)] public float initSpeed = 4.0f;

    // 카메라 변수
    [System.NonSerialized] public float groundY = 0.0f;

    // 저장 데이터 파라미터
    public static bool checkPointEnabled = false;
    public static string checkPointSceneName = "";
    public static string checkPointLabelName = "";

    // 외부로부터 처리를 조작하기 위한 파라미터
    public static bool initParam = true;

    // 내부 파라미터
    public int char_localScale = 0; // 물건 움직일때 캐릭터 방향 안바꾸기

    int jumpCount = 0;
    bool breakEnabled = true;
    float groundFriction = 0.8f;

    // 코드 지원 함수
    public static GameObject GetGameObject()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }
    public static Transform GetTransform()
    {
        return GameObject.FindGameObjectWithTag("Player").transform;
    }
    public static PlayerController GetController()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    public static Animator GetAnimator()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }


    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        speed = initSpeed;
        groundY = groundCheck_C.transform.position.y + 2.0f;

        // 파라미터 초기화
        if (initParam) // 책에 있는 SetHP 필요없음
            initParam = false;
        //책에 있는 HP가 없을 때 1부터 다시 시작하는 코드도 필요 없음

        // 이게 있으면 스테이지 넘어갔을때 안움직여짐
        //if(checkPointEnabled)
        //{
        //    StageTrigger_CheckPoint[] triggerList = GameObject.Find("Stage").GetComponentsInChildren<StageTrigger_CheckPoint>();
        //    foreach(StageTrigger_CheckPoint trigger in triggerList)
        //    {
        //        if (trigger.labelName == checkPointLabelName)
        //        {
        //            transform.position = trigger.transform.position;
        //            groundY = transform.position.y;
        //            Camera.main.GetComponent<CameraFollow>().SetCamera(trigger.cameraParam);
        //            break;
        //        }
        //    }
        //}

        Camera.main.transform.position = new Vector3(transform.position.x, groundY, Camera.main.transform.position.z);

    }


    protected override void FixedUpdateCharacter()
    {
        // 착지 검사
        if (jumped)
            if((grounded && !groundedPrev) || (grounded && Time.fixedTime > jumpStartTime + 1.0f))
            {
                animator.SetTrigger("Idle");
                jumped = false;
                jumpCount = 0;
            }

        if (!jumped)
            jumpCount = 0;

        // 캐릭터 방향
        if (char_localScale == 0)
            transform.localScale = new Vector3(basScaleX * dir, transform.localScale.y, transform.localScale.z);
        else if (char_localScale == 1)
            transform.localScale = new Vector3(basScaleX * 1, transform.localScale.y, transform.localScale.z);
        else if (char_localScale == 2)
            transform.localScale = new Vector3(basScaleX * -1, transform.localScale.y, transform.localScale.z);


        // 점프 도중에 가로 이동 감속
        if (jumped && !grounded)
            if(breakEnabled)
            {
                breakEnabled = false;
                speedVx *= 0.9f;
            }

        // 이동 정지(감속) 처리__ 캐릭터가 정지할때 약간 미끄러지는가?
        if (breakEnabled)
            speedVx *= groundFriction;



        // 카메라
        //cameraPosition = (transform.position + Vector3.back);
        //cameraPosition.y += 3.14f;
        //Camera.main.transform.position = cameraPosition;
 
    }
    

    // 기본 액션
    public override void ActionMove(float n)
    {
        if (!activeSts)
            return;

        // 초기화
        float dirOld = dir;
        breakEnabled = false;

        // 애니메이션 지정
        float moveSpeed = Mathf.Clamp(Mathf.Abs(n), -1.0f, 1.0f);
        animator.SetFloat("MovSpeed", moveSpeed);
        // animatorr.speed = 1.0f + moveSpeed;

        // 이동 체크
        if (n != 0.0f)
        {
            // 이동
            dir = Mathf.Sign(n);
            moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;
            speedVx = initSpeed * moveSpeed * dir;
        }
        else
            breakEnabled = true;  // 이동 정지

        // 시점에서 돌아보기 검사
        if (dirOld != dir)
            breakEnabled = true;
    }

    public override void Dead()
    {
        if (dir != 1)
            animator.SetBool("right_dead", true);
        else
            animator.SetBool("left_dead", true);
    }


    public void ActionJump()
    {
        if (grounded)
        {
            animator.SetTrigger("Jump");
            rigid.velocity = Vector2.up * 13.0f;
            jumpStartTime = Time.fixedTime;
            jumped = true;
        }

    }
}
