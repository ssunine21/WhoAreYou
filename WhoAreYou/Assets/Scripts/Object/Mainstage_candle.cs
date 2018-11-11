using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainstage_candle : MonoBehaviour
{
    public float gravity = 0.03f;

    Rigidbody2D rid2d;
    Animator anim;
    Collider2D col2d;

    // Use this for initialization
    void Awake()
    {
        rid2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col2d = GetComponent<Collider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerBodyCollider.main_switch_check)
            this.rid2d.gravityScale += gravity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }
}
