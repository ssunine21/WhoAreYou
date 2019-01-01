using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public float moveSpeed = 5.0f;

	private float h = 0.0f;
	private Transform tr;
	private Animator animator;
	private SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
		tr = GetComponent<Transform>();
		animator = GetComponent<Animator>();
		render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		h = Input.GetAxis("Horizontal");

		if ( h != 0 ) {
			animator.SetBool("isRun", true);
			tr.Translate(Vector2.right * moveSpeed * h * Time.deltaTime, Space.Self);
			if ( h < 0 )
				render.flipX = true;
			else
				render.flipX = false;
		}
		else {
			animator.SetBool("isRun", false);
		}
		Debug.Log("h = " + h.ToString());

    }


}
