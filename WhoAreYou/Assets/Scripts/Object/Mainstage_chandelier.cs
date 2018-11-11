using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainstage_chandelier : MonoBehaviour
{
    public float gravity = 0.03f;

    Rigidbody2D rid2d;
    Animator anim;

    bool dead = true;


    // Use this for initialization
    void Awake ()
    {
        rid2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (PlayerBodyCollider.main_switch_check)
        {
            this.rid2d.gravityScale += gravity;
            anim.SetBool("isChandelier", true);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Road"))
        {
            anim.SetBool("isBoom", true);
            this.rid2d.gravityScale = 0.0f;
            dead = false;
        }
        else if(other.CompareTag("PlayerBody") && dead)
        {
            other.gameObject.GetComponentInParent<PlayerController>().Dead();
        }
    }
}
