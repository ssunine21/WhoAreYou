using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour {

    public string stage;
    public GameObject portal;
    public Image fade;

    Inventory inven;
    CameraFollow camera_follow;
    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_btnB;

    DialogueManager dMag;

    bool fadeout = false;
    bool fadein = false;
    float time = 0.0f;
    float fades = 0.0f;

    static public bool portal_rockA = false;

    // Use this for initialization
    void Start()
    {
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
        inven = GameObject.FindObjectOfType<Inventory>();
        camera_follow = GameObject.Find("mainStage_MainCamera").GetComponent<CameraFollow>();
        dMag = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    
    private void Update()
    {
        vpad_btnB = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
        {
            vpad_btnB = vpad.buttonB;
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

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "PlayerBody")
        {
            if (fades >= 1.0f)
            {
                Debug.Log("포탈");
                Vector3 portal_position = portal.transform.position;
                other.transform.parent.position = portal_position;

                if (stage == "stageMain")
                    camera_follow.SetMaxXpos(1);
                else if (stage == "stageB")
                    camera_follow.SetMaxXpos(2);
                else if (stage == "stageA")
                    camera_follow.SetMaxXpos(4);
            }

            if (Input.GetKeyDown(KeyCode.X) || vpad_btnB == zFOXVPAD_BUTTON.DOWN)
            {
                if (gameObject.name == "Portal_B")
                {
                    for (int i = 0; i < inven.AllSlot.Count; ++i)
                    {
                        Slot slot = inven.AllSlot[i].GetComponent<Slot>();

                        if (slot.isSlots() == false)
                        {
                            if (!dMag.dialogActive)
                            {
                                dMag.currentLine = 0;
                                dMag.ShowBox("열리지 않아... 맞는 " + "<color=#ff0000>" + "열쇠" + "</color>" + "가 있을까??");
                                break;
                            }
                        }

                        else if (slot.ItemReturn().type.ToString() == "Key")
                        {
                            fadeout = true;
                            break;
                        }
                    }
                }
                else if (gameObject.name == "PortalAtoMain")
                {
                    if (portal_rockA)
                    {
                        if (!dMag.dialogActive)
                        {
                            dMag.currentLine = 0;
                            dMag.ShowBox("문이 잠겼어...");
                        }
                    }

                    else 
                    {
                        if (FireTrriger.Start_pierrot)
                        {
                            for (int i = 0; i < inven.AllSlot.Count; ++i)
                            {
                                Slot slot = inven.AllSlot[i].GetComponent<Slot>();

                                if (slot.isSlots() == false)
                                {
                                    if (!dMag.dialogActive)
                                    {
                                        dMag.currentLine = 0;
                                        dMag.ShowBox("테이블 위에 저게 뭐지?");
                                        break;
                                    }
                                }

                                else if (slot.ItemReturn().type.ToString() == "Key")
                                {
                                    fadeout = true;
                                    break;
                                }
                            }
                        }
                        else
                            fadeout = true;
                    }
                }
                else
                {
                    fadeout = true;
                }
            }
        }
    }
}
