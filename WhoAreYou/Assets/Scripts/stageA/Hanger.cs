using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanger : MonoBehaviour
{

    DialogueManager dMag;
    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_btnB;

    public GameObject player;
    public GameObject vpadset;
    public GameObject pierrot;
    public GameObject candle_light;
    public GameObject goldKey;
    public GameObject pierrotLight;
    public GameObject charLight;

    public UnityEngine.UI.Image fade;

    bool move = false;
    bool camera_pierrot_move = false;
    bool camera_pierrot_open = false;
    bool fadein = false;
    bool fadeout = false;

    static public bool end = false;

    float time = 0.0f;
    float fades = 0.0f;

    Vector3 cameraV3;
    Vector3 pierrotV3;
    CameraFollow camera_follow;
    Animator anim;

    // Use this for initialization
    private void Awake()
    {
        dMag = FindObjectOfType<DialogueManager>();
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
        camera_follow = GameObject.Find("mainStage_MainCamera").GetComponent<CameraFollow>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        vpad_btnB = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
        {
            vpad_btnB = vpad.buttonB;
        }

        if (move)
        {
            // -57.69949, 10
            if (cameraV3.x > 10.0f)
            {
                camera_follow.transform.position = cameraV3;
                StartCoroutine("PierrotCall");
            }
            else if (cameraV3.x > 7.0f)
            {
                cameraV3.x += 0.03f;
                camera_follow.transform.position = cameraV3;
            }

            else
            {
                cameraV3.x += 0.1f;
                camera_follow.transform.position = cameraV3;
            }
        }

        if (camera_pierrot_move)
        {
            if (cameraV3.x > 0.27f)
                cameraV3.x -= 0.05f;
            else
                camera_follow.transform.position = cameraV3;

            if (pierrotV3.x > -2.6f)
                pierrotV3.x -= 0.05f;
            else
            {
                pierrot.transform.position = pierrotV3;
                pierrot.GetComponent<Animator>().SetBool("isRun", false);
                camera_pierrot_move = false;
                StartCoroutine("Openhanger");
            }
            camera_follow.transform.position = cameraV3;
            pierrot.transform.position = pierrotV3;
        }

        if (camera_pierrot_open)
        {
            pierrot.GetComponent<Animator>().SetBool("isRun", true);
            
            if (pierrotV3.x > -10.0f)
            {
                pierrotV3.x -= 0.04f;
            }
            else
            {
                pierrot.SetActive(false);
                camera_pierrot_open = false;
                StartCoroutine("Openhanger2");
            }

            pierrot.transform.position = pierrotV3;
        }

        if (fadeout)
        {
            time += Time.deltaTime;
            if (fades < 1.0f && time >= 0.1f)
            {
                fades += 0.12f;
                fade.color = new Color(0, 0, 0, fades);
                time = 0;
            }
            else if (fades >= 1.0f)
            {
                anim.SetBool("isIng", true);
                player.SetActive(true);
                vpadset.SetActive(true);
                pierrotLight.SetActive(false);
                charLight.SetActive(true);
                Portal.portal_rockA = false;
                end = true;

                fadeout = false;
                fadein = true;
            }
        }
        else if (fadein)
        {
            time += Time.deltaTime;
            if (fades > 0.0f && time >= 0.1f)
            {
                fades -= 0.06f;
                fade.color = new Color(0, 0, 0, fades);
                time = 0;
            }
            else if (fades <= 0.0f)
            {
                fadein = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            if (Input.GetKeyDown(KeyCode.X) || vpad_btnB == zFOXVPAD_BUTTON.DOWN)
            {
                if (!dMag.dialogActive)
                {
                    if (!FireTrriger.Start_pierrot)
                    {
                        dMag.currentLine = 0;
                        dMag.ShowBox("옷장이 굉장히 크네");
                    }
                    else if (FireTrriger.Start_pierrot && Match.SetMask && !Match.Setcandle)
                    {
                        if (!end)
                            ExitStart();
                        else
                        {
                            dMag.currentLine = 0;
                            dMag.ShowBox("들킬뻔 했어..");
                        }
                            
                    }
                    else
                    {
                        dMag.currentLine = 0;
                        dMag.ShowBox("이대로 숨으면 들킬꺼야!");
                    }
                }
            }
        }
    }

    void ExitStart()
    {
        player.SetActive(false);
        vpadset.SetActive(false);
        // play audio
        StartCoroutine("CameraMove");
    }

    IEnumerator CameraMove()
    {
        yield return new WaitForSeconds(2.0f);
        cameraV3 = camera_follow.transform.position;
        move = true;
    }

    bool pcall1 = true;
    bool pcall2 = true;
    bool pcall3 = true;
    bool pcall4 = true;
    bool pcall5 = true;

    IEnumerator PierrotCall()
    { // 9.2, -61

        Vector3 pierrotV3 = new Vector3(9.2f, -61, 0);
        yield return new WaitForSeconds(3.0f);
        pierrot.SetActive(true);
        candle_light.SetActive(true);
        if (pcall1)
        {
            pierrot.GetComponent<SpriteRenderer>().flipX = false;
            pierrot.transform.position = pierrotV3;
        }

        StartCoroutine("PCall1");

    }

    IEnumerator PCall1()
    {
        yield return new WaitForSeconds(1.0f);
        pcall1 = false;
        if (pcall2)
        {
            pierrot.GetComponent<Animator>().SetBool("isThink", true);
        }

        yield return new WaitForSeconds(3.0f);
        goldKey.SetActive(true);

        StartCoroutine("PCall2");
    }

    IEnumerator PCall2()
    {
        yield return new WaitForSeconds(1.0f);
        pcall2 = false;
        if (pcall3)
        {
            pierrot.GetComponent<Animator>().SetBool("isThink", false);
            pierrot.GetComponent<SpriteRenderer>().flipX = true;
            pierrot.GetComponent<Animator>().SetBool("isHead", true);
        }
        StartCoroutine("PCall3");
    }

    IEnumerator PCall3()
    {
        yield return new WaitForSeconds(2.5f);
        pcall3 = false;
        if (pcall4)
        {
            pierrot.GetComponent<Animator>().SetBool("isHeadthink", true);
            pierrot.GetComponent<Animator>().SetBool("isHead", false);
        }

        StartCoroutine("PCall4");
    }

    IEnumerator PCall4()
    {
        yield return new WaitForSeconds(1.0f);
        pcall4 = false;
        if (pcall5)
        {
            pierrot.GetComponent<Animator>().SetBool("isRun", true);
            pierrot.GetComponent<Animator>().SetBool("isHeadthink", false);
            camera_pierrot_move = true;
            pierrotV3 = pierrot.transform.position;
        }
        pcall5 = false;
        move = false;
    }

    IEnumerator Openhanger()
    {
        yield return new WaitForSeconds(2.0f);
        pierrot.GetComponent<Animator>().SetBool("isSearch", true);

        yield return new WaitForSeconds(2.5f);
        camera_follow.param.screenOGSize = 3.0f;
        pierrot.GetComponent<Animator>().SetBool("isOpen", true);
        anim.SetBool("isOpen", true);
        pierrot.GetComponent<Animator>().SetBool("isSearch", false);
        yield return new WaitForSeconds(0.2f);
        camera_follow.param.screenOGSize = 5.0f;

        yield return new WaitForSeconds(3.0f);
        pierrot.transform.DetachChildren();
        camera_pierrot_open = true;
    }

    IEnumerator Openhanger2()
    {
        yield return new WaitForSeconds(2.5f);
        anim.SetBool("isLeft", true);

        yield return new WaitForSeconds(1.0f);
        fadeout = true;
    }
}
