using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour {

    // 캐쉬
    PlayerController playerCtrl;
    zFoxVirtualPad vpad;

    // Use this for initialization
    void Awake()
    {
        playerCtrl = GetComponent<PlayerController>();
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCtrl.activeSts)
            return;

        // 가상 패드
        float vpad_vertical = 0.0f;
        float vpad_horizontal = 0.0f;
        zFOXVPAD_BUTTON vpad_btnA = zFOXVPAD_BUTTON.NON;
        zFOXVPAD_BUTTON vpad_btnB = zFOXVPAD_BUTTON.NON;
        if (vpad != null)
        {
            vpad_vertical = vpad.vertical;
            vpad_horizontal = vpad.horizontal;
            vpad_btnA = vpad.buttonA;
            vpad_btnB = vpad.buttonB;
        }

        // 이동
        float joyMv = Input.GetAxisRaw("Horizontal");
        joyMv = Mathf.Pow(Mathf.Abs(joyMv), 3.0f) * Mathf.Sign(joyMv);

        float vpadMv = vpad_horizontal;
        vpadMv = Mathf.Pow(Mathf.Abs(vpadMv), 1.5f) * Mathf.Sign(vpadMv);
        playerCtrl.ActionMove(joyMv + vpadMv);

        // 패드 처리
        //playerCtrl.ActionMove(joyMv);

        // 점프
        if (Input.GetKeyDown(KeyCode.Space) || vpad_btnA == zFOXVPAD_BUTTON.DOWN)
        {
            playerCtrl.ActionJump();
            return;
        }
    }
}
