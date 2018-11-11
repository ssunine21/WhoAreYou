using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public GameObject candle_light;
    public GameObject player_light;
    public GameObject mask;
    public GameObject pierrot;

    DialogueManager dMag;
    Inventory inven;
    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_btnB;

    public string[] dialogueLines;

    static public bool Setcandle = false;
    static public bool SetMask = false;
    
    void Awake()
    {
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
        inven = GameObject.FindObjectOfType<Inventory>();
        dMag = FindObjectOfType<DialogueManager>();
        pierrot.gameObject.GetComponent<Pierrot>();
    }

    private void Update()
    {
        vpad_btnB = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
        {
            vpad_btnB = vpad.buttonB;
        }
    }

    // 충돌체크
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            if (Input.GetKeyDown(KeyCode.X) || vpad_btnB == zFOXVPAD_BUTTON.DOWN)
            {
                if (!FireTrriger.Start_pierrot)
                {
                    for (int i = 0; i < inven.AllSlot.Count; ++i)
                    {
                        Slot slot = inven.AllSlot[i].GetComponent<Slot>();

                        if (slot.isSlots() == false)
                        {
                            if (!dMag.dialogActive)
                            {
                                dMag.currentLine = 0;
                                dMag.ShowBox("<color=#ff0000>" + "초를" + "</color>" + " 켤 만한 걸 찾아보자");
                                break;
                            }
                        }

                        else if (slot.ItemReturn().type.ToString() == "Match")
                        {
                            candle_light.SetActive(true);
                            Setcandle = true;
                            break;
                        }
                    }
                }
                else if (FireTrriger.Start_pierrot) //불이 켜진상태
                {
                    if(Hanger.end)
                    {
                        dMag.currentLine = 0;
                        dMag.ShowBox("불은 켜두는게 좋겠어");
                    }

                    else if (!SetMask)
                    {
                        for (int i = 0; i < inven.AllSlot.Count; ++i)
                        {
                            Slot slot = inven.AllSlot[i].GetComponent<Slot>();
                            if (slot.isSlots() == false)
                            {
                                SetMask = false;
                            }
                            else if (slot.ItemReturn().type.ToString() == "Mask")
                            {
                                SetMask = true;
                                break;
                            }
                        }
                    }

                    if (Setcandle && !SetMask) // 껐는데 마스크가 없다면
                    {
                        candle_light.SetActive(false);
                        player_light.SetActive(false);
                        mask.SetActive(false);
                        Setcandle = false;


                    }
                    else if (Setcandle && SetMask) // 껐는데 마스크가 있다면
                    {
                        candle_light.SetActive(false);
                        player_light.SetActive(false);
                        Setcandle = false;
                    }
                    else if (!Setcandle && SetMask) // 마스크가 있어서 초를 끄고 숨을 수 있을 때
                    {
                        if (!dMag.dialogActive)
                        {
                            dMag.currentLine = 0;
                            dMag.ShowBox("좋아 숨자");
                        }
                    }
                    else if (!Setcandle && !SetMask) // 마스크가 없어서 초를 다시 킬 때
                    {
                       candle_light.SetActive(true);
                        pierrot.SetActive(true);
                        StartCoroutine("gameOver");
                    }
                }
            }
        }
    }

    IEnumerator gameOver()
    {
        yield return new WaitForSeconds(2.0f);
        pierrot.gameObject.GetComponent<Animator>().SetBool("isShout", true);

        yield return new WaitForSeconds(3.0f);
        GameOver.game_over = true;
    }
}