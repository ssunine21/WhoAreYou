using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAction : MonoBehaviour {

    GameObject player;
    GameObject playerEquip;

    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_btnB;
    
    bool isPlayerEnter = false;

    // Use this for initialization
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerBody");
        playerEquip = GameObject.FindGameObjectWithTag("Equip");
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
    }

    // Update is called once per frame
    void Update ()
    {
        vpad_btnB = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
            vpad_btnB = vpad.buttonB;

        if(vpad_btnB == zFOXVPAD_BUTTON.DOWN)
        {
            if (isPlayerEnter)
            {
                transform.SetParent(playerEquip.transform);
                transform.localPosition = Vector3.zero;
                isPlayerEnter = false;
            }
            else
            {
                playerEquip.transform.DetachChildren();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
            isPlayerEnter = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
            isPlayerEnter = false;
    }
}
