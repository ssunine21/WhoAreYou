using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramove : MonoBehaviour {
    
    public GameObject keypad;
    public GameObject monster;

    CameraFollow camera_follow;
    PlayerController player_ctr;
    monsterMove monster_move;

    bool move = false;
    bool screesix = true;

    Vector3 v3;
    //ClassC c = GameObject.Find("objectA").GetComponent<ClassC>();
    // Use this for initialization
    void Start ()
    {
        camera_follow = GameObject.Find("mainStage_MainCamera").GetComponent<CameraFollow>();
        player_ctr = GameObject.Find("Player").GetComponent<PlayerController>();
        monster_move = monster.GetComponent<monsterMove>();
        v3 = camera_follow.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (move)
        {
            if (v3.x < 6.0f)
            {
                camera_follow.transform.position = v3;
                StartCoroutine("monsterCall_one");
            }
            else if (v3.x < 10.0f)
            {
                v3.x -= 0.03f;
                camera_follow.transform.position = v3;
            }
            // 29에서 8까지 21차이
            else
            {
                v3.x -= 0.5f;
                camera_follow.transform.position = v3;
                camera_follow.param.screenOGSize = 7;
                player_ctr.initSpeed = 0;
            }
        }
    }

    IEnumerator monsterCall_one()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("2초대기");
        if (screesix)
            camera_follow.param.screenOGSize = 5.9f;

        monster_move.SetCall();

        yield return new WaitForSeconds(0.8f);
        screesix = false;
        camera_follow.param.screenOGSize = 7;

        yield return new WaitForSeconds(2.0f);
        monster_move.SetRun();

        yield return new WaitForSeconds(1.9f);
        keypad.SetActive(true);

        player_ctr.initSpeed = 7.0f;
        move = false;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerBody"))
        {
            monster.SetActive(true);
            v3 = camera_follow.transform.position;
            Debug.Log("플레이어 부딪힘");
            keypad.SetActive(false);
            move = true;
        }
    }
}
