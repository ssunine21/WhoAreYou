using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{

    public GameObject match;
    public GameObject goldKey;

    Animator anim;
    Rigidbody2D rigid2d;
    Inventory inven;

    bool check = false;
    int count = 0;

    private void Start()
    {
        goldKey.SetActive(false);   
    }

    private void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inven = GameObject.FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        // 불 켜지면 아이템 스크립트 켜지기
        if(FireTrriger.Start_pierrot)
            this.gameObject.GetComponent<Item>().enabled = true;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isAttack", true);


            if (count == 3)
            {
                rigid2d.constraints = RigidbodyConstraints2D.None;
                match.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                check = true;
            }
            else
                count++;

            if (!check)
                StartCoroutine("AttackTime");
        }
    }

    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("isAttack", false);
    }
}