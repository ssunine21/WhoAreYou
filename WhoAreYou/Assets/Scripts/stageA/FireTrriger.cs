using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrriger : MonoBehaviour {

    public GameObject keypad;

    CameraFollow camera_follow;
    PlayerController player_ctr;
    Vector3 v3;
    DialogueManager dMag;

    static public bool Start_pierrot;
    bool move = false;
    bool screen_trigger1 = true; // 카메라 쿵쾅쿵쾅효과 트리거
    bool screen_trigger2 = true;
    bool screen_trigger3 = true;
    bool screen_trigger4 = true;

    // Use this for initialization
    private void Awake()
    {
        Start_pierrot = false;

        camera_follow = GameObject.Find("mainStage_MainCamera").GetComponent<CameraFollow>();
        player_ctr = GameObject.Find("Player").GetComponent<PlayerController>();
        dMag = FindObjectOfType<DialogueManager>();

        StartCoroutine("Startpierrot");
        v3 = camera_follow.transform.position;
    }

    void Update()
    {
        if (move)
        {
            if (v3.x < -1.0f)
            {
                camera_follow.transform.position = v3;
                StartCoroutine("pierrotCall_one");
            }
            else if (v3.x < 0.5f)
            {
                v3.x -= 0.03f;
                camera_follow.transform.position = v3;
            }

            else
            {
                v3.x -= 0.5f;
                camera_follow.transform.position = v3;
                player_ctr.initSpeed = 0;
            }
        }
    }

    IEnumerator Startpierrot()
    {
        yield return new WaitForSeconds(1.0f);
        player_ctr.dir = -1;
        keypad.SetActive(false);

        v3 = camera_follow.transform.position;
        move = true;
    }

    IEnumerator pierrotCall_one()
    {
        yield return new WaitForSeconds(0.5f);
        if (screen_trigger1)
            camera_follow.param.screenOGSize = 4.7f;

        yield return new WaitForSeconds(0.3f);
        screen_trigger1 = false;
        if (screen_trigger2)
            camera_follow.param.screenOGSize = 4.9f;

        yield return new WaitForSeconds(0.4f);
        screen_trigger2 = false;
        if (screen_trigger3)
            camera_follow.param.screenOGSize = 4.6f;

        yield return new WaitForSeconds(0.3f);
        screen_trigger3 = false;
        if (screen_trigger4)
            camera_follow.param.screenOGSize = 4.8f;
        screen_trigger4 = false;

        yield return new WaitForSeconds(0.3f);

        if (!dMag.dialogActive)
        {
            dMag.currentLine = 0;
            dMag.ShowBox("누군가가 오고있어..!!");
        }
        
        keypad.SetActive(true);
        player_ctr.initSpeed = 4.0f;
        camera_follow.param.screenOGSize = 5.0f;

        Start_pierrot = true;

        move = false;
    }
}
