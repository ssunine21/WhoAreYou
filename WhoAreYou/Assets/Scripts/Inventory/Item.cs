using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public enum TYPE { Key, Match, Mask, other }

    public TYPE type;           // 아이템의 타입.
    public Sprite DefaultImg;   // 기본 이미지.
    public int MaxCount;        // 겹칠수 있는 최대 숫자.
    
    DialogueManager dMag;
    public string[] dialogueLines;

    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_btnB;

    public GameObject mark;

    // 인벤토리에 접근
    private Inventory Iv;

    void Awake()
    {
        // 태그명이 "Inventory"인 객체의 GameObject를 반환한다.
        // 반환된 객체가 가지고 있는 스크립트를 GetComponent를 통해 가져온다.
        Iv = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
        dMag = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        vpad_btnB = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
        {
            vpad_btnB = vpad.buttonB;
        }
    }

    void AddItem()
    {
        if (!Iv.AddItem(this))
            Debug.Log("아이템이 가득 참");
        else
            gameObject.SetActive(false); // 아이템을 비활성화 시켜준다.
    }

    // 충돌체크
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBody")
        {
            // 플레이어와 충돌하면.
            if (Input.GetKeyDown(KeyCode.X) || vpad_btnB == zFOXVPAD_BUTTON.DOWN)
            {
                mark.SetActive(false);
                AddItem();

                
                //dMag.ShowBox(dialogue);
                if (!dMag.dialogActive && dialogueLines.Length != 0)
                {
                    dMag.dialogLines = dialogueLines;
                    dMag.currentLine = 0;
                    dMag.ShowDialogue();
                }
            }
        }
    }

}
