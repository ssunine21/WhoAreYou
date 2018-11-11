using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndA : MonoBehaviour
{

    zFoxVirtualPad vpad;
    CameraFollow A_camera;
    PlayerController playerCtr;

    public GameObject mark;

    // Use this for initialization
    void Awake()
    {
        A_camera = GameObject.FindObjectOfType<CameraFollow>();
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
        playerCtr = GameObject.FindObjectOfType<PlayerController>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            playerCtr.GetComponent<Animator>().SetBool("Idle 0", true);
            playerCtr.GetComponent<PlayerController>().initSpeed = 0.0f;
            playerCtr.GetComponent<SpriteRenderer>().flipX = true;

            A_camera.param.margin.x = -0.5f;

            StartCoroutine("AfterDoorLock");
        }
    }
    
    IEnumerator AfterDoorLock()
    {
        yield return new WaitForSeconds(1.5f);
        vpad.gameObject.SetActive(true);

        playerCtr.GetComponent<Animator>().SetBool("Idle 0", false);
        playerCtr.GetComponent<PlayerController>().initSpeed = 4.0f;
        playerCtr.GetComponent<SpriteRenderer>().flipX = false;
        
        A_camera.param.margin.x = 2.0f;
        this.gameObject.SetActive(false);

        // 문 잠그기
        Portal.portal_rockA = true;
    }
}