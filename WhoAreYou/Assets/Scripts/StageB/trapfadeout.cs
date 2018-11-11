using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapfadeout : MonoBehaviour {

    public UnityEngine.UI.Image fade;
    public string[] dialogueLines;

    DialogueManager dMag;
    Vector3 v3;
    CameraFollow camera_follow;
    PlayerController player_ctr;

    bool fadein = false;
    float time = 0.0f;
    float fades = 1.0f;

    int fadecheck = 0;

    // Use this for initialization
    void Awake () {
        dMag = FindObjectOfType<DialogueManager>();
        v3.x = 113.78f; v3.y = -40.2f; v3.z = 0.0f;
        camera_follow = GameObject.Find("mainStage_MainCamera").GetComponent<CameraFollow>();
        player_ctr = GameObject.Find("Player").GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {

        if (fadein && !dMag.dialogActive)
        {
            time += Time.deltaTime;
            if (fades > 0.0f && time >= 0.1f)
            {
                fades -= 0.08f;
                fade.color = new Color(0, 0, 0, fades);
                time = 0;
            }
            else if (fades <= 0.0f)
            {
                fadein = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        fade.color = new Color(0, 0, 0);
        Time.timeScale = 1.0f;
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = v3;

            if (fadecheck == 0)
                fade.color = new Color(0, 0, 0);
            StartCoroutine("AfterTrap");
            camera_follow.SetMaxXpos(3);
            player_ctr.initSpeed = 4.0f;

            fadecheck++;
        }

    }

    IEnumerator AfterTrap()
    {
        yield return new WaitForSeconds(3.0f);
        
        if (!dMag.dialogActive)
        {
            Debug.Log("CoroutineAction");
            dMag.dialogLines = dialogueLines;
            dMag.currentLine = 0;
            dMag.ShowDialogue();
        }
        fadein = true;
    }
}
