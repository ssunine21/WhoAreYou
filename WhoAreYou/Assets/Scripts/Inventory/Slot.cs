using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

    public Stack<Item> slot;       // 슬롯을 스택으로 만든다.
    public Sprite DefaultImg; // 슬롯에 있는 아이템을 다 사용할 경우 아무것도 없는 이미지를 넣어줄 필요가 있다.

    private Image ItemImg;
    private bool isSlot;     // 현재 슬롯이 비어있는지?

    public Item ItemReturn() { return slot.Peek(); } // 슬롯에 존재하는 아이템이 뭔지 반환. 
    public bool isSlots() { return isSlot; } // 슬롯이 현재 비어있는지? 에 대한 플래그 반환.
    public void SetSlots(bool isSlot) { this.isSlot = isSlot; }

    void Start()
    {
        // 스택 메모리 할당.
        slot = new Stack<Item>();

        // 맨 처음엔 슬롯이 비어있다.
        isSlot = false;

        ItemImg = transform.GetChild(0).GetComponent<Image>();
    }

    public void AddItem(Item item)
    {
        // 스택에 아이템 추가.
        slot.Push(item);
        UpdateInfo(true, item.DefaultImg);
    }

    // 아이템 사용.
    public void ItemUse()
    {
        // 슬롯이 비어있으면 함수를 종료.
        if (!isSlot)
            return;

        // 슬롯에 아이템이 1개인 경우.
        // 아이템이 1개일 때 사용하게 되면 0개가 된다.
        if (slot.Count == 1)
        {
            // 혹시 모를 오류를 방지하기 위해 slot리스트를 Clear해준다
            slot.Clear();
            UpdateInfo(false, DefaultImg);
            return;
        }

        slot.Pop();
        UpdateInfo(isSlot, ItemImg.sprite);
    }

    // 슬롯에 대한 각종 정보 업데이트.
    public void UpdateInfo(bool isSlot, Sprite sprite)
    {
        SetSlots(isSlot);                                          // 슬롯이 비어있다면 false 아니면 true 셋팅.
        ItemImg.sprite = sprite;                                   // 슬롯안에 들어있는 아이템의 이미지를 셋팅.
        
    }
}
