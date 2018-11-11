using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCollider : MonoBehaviour {


    PlayerController playerCtrl;
    Animator other_animator;
    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_btnB;
    Slot item;

    static bool markSet = false;
    public GameObject mark;
    public static bool main_switch_check;

    //시작할 때 인벤토리 닫기
    private void Start()
    {
        GameObject.Find("Inventory Panal").SetActive(false);
    }

    void Awake()
    {
        playerCtrl = transform.parent.GetComponent<PlayerController>();
        item = GetComponent<Slot>();
        main_switch_check = false;
        

        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
    }

    private void Update()
    {
        vpad_btnB = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
        {
            vpad_btnB = vpad.buttonB;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "stairs" && (other.transform.position.y) > this.transform.position.y)
            other.collider.isTrigger = true;
    }

    //이건 뭘 읠미하는 것일까
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!playerCtrl.jumped &&
            (other.gameObject.tag == "Road" ||
            other.gameObject.tag == "MoveObject" ||
            other.gameObject.tag == "stairs"))
        {
            playerCtrl.groundY = transform.parent.transform.position.y;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("stairs") && (other.transform.position.y) < this.transform.position.y)
        {
            other.isTrigger = false;
        }

        //else if (other.tag == "CameraTrigger")
        //{
        //    Camera.main.GetComponent<CameraFollow>().SetCamera(other.GetComponent<StageTrigger_Camera>().param);
        //}

        //if (other.CompareTag("EventTrigger"))
        //{
        //    other.gameObject.SendMessage("OnTriggerEnter2D_PlayerEvent", gameObject);
        //}

        // 이게 왜 있는 걸까 이게 없으면 스위치 안돌아감
        other_animator = other.gameObject.GetComponentInChildren<Animator>();

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("main_switch"))
        {
            mark.SetActive(true);
            markSet = true;
            if (Input.GetKeyDown(KeyCode.X) || vpad_btnB == zFOXVPAD_BUTTON.DOWN)
            {
                other_animator.SetBool("isSwitch", true);
                main_switch_check = true;
            }
        }
        else if (other.CompareTag("Item") || other.CompareTag("Object") || other.CompareTag("EventTrigger"))
        {
            mark.SetActive(true);
            markSet = true;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (markSet)
        {
            mark.SetActive(false);
            markSet = false;
        }
    }

}
