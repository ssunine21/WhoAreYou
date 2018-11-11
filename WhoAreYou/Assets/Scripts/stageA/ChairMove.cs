using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairMove : MonoBehaviour
{

    GameObject player;
    GameObject playerEquip;

    Animator anim;
    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_btnB;
    
    bool isPlayerEnter = false;

    // Use this for initialization
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerBody");
        playerEquip = GameObject.FindGameObjectWithTag("Equip");
        anim = GetComponent<Animator>();
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
    }

    // Update is called once per frame
    void Update()
    {
        vpad_btnB = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
            vpad_btnB = vpad.buttonB;

        if (isPlayerEnter && vpad_btnB == zFOXVPAD_BUTTON.DOWN)
        {
            player.gameObject.GetComponentInParent<Animator>().SetBool("isCatch", true);

            transform.SetParent(playerEquip.transform);
            transform.localPosition = Vector3.zero;
            if (player.gameObject.GetComponentInParent<PlayerController>().dir == 1)
                player.gameObject.GetComponentInParent<PlayerController>().char_localScale = 1;
            else
                player.gameObject.GetComponentInParent<PlayerController>().char_localScale = 2;

            player.gameObject.GetComponentInParent<PlayerController>().initSpeed = 2.0f;
            isPlayerEnter = false;
        }
        else if (vpad_btnB == zFOXVPAD_BUTTON.UP)
        {
            player.gameObject.GetComponentInParent<Animator>().SetBool("isCatch", false);

            player.gameObject.GetComponentInParent<PlayerController>().char_localScale = 0;
            player.gameObject.GetComponentInParent<PlayerController>().initSpeed = 4.0f;

            playerEquip.transform.DetachChildren();

            //if (player.gameObject.GetComponentInParent<PlayerController>().dir == 1)
            //    player.gameObject.GetComponentInParent<PlayerController>().dir = -1;
            //else
            //    player.gameObject.GetComponentInParent<PlayerController>().dir = 1;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isPlayerEnter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
            isPlayerEnter = false;
    }
}
