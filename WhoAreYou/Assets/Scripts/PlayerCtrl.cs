using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public float moveSpeed = 5.0f;
	public float jumpPower = 10.0f;

	private bool isJump = false;

	private float h = 0.0f;
	private Transform tr;
	private Animator animator;
	private SpriteRenderer render;
	private Rigidbody2D rigid;

	private GameObject mark;

	private readonly int hashMove = Animator.StringToHash("isMove");
	private readonly int hashJump = Animator.StringToHash("isJump");
	private readonly int hashdoJump = Animator.StringToHash("doJump");


	// Start is called before the first frame update
	void Start()
    {
		tr = GetComponent<Transform>();
		animator = GetComponent<Animator>();
		render = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		mark = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		h = Input.GetAxisRaw("Horizontal");
    }

	private void FixedUpdate() {
		Move();

		if ( Input.GetButtonDown("Jump") ){
			Jump();
		}
	}

	private void Move() {
		if ( h != 0 ) {
			animator.SetBool(hashMove, true);
			tr.Translate(Vector2.right * moveSpeed * h * Time.deltaTime, Space.Self);
			if ( h < 0 )
				render.flipX = true;
			else
				render.flipX = false;
		}
		else {
			animator.SetBool(hashMove, false);
		}
	}

	private void Jump() {
		if ( isJump )
			return;

		isJump = true;
		animator.SetBool(hashJump, true);
		animator.SetTrigger(hashdoJump);
		rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
	}

	private void OnTriggerEnter2D( Collider2D other ) {
		if ( other.gameObject.layer == 8 && rigid.velocity.y <= 0 ) {
			isJump = false;
			animator.SetBool(hashJump, false);
		}

		else if ( FindObject(other) ) {
			mark.SetActive(true);
		}
	}

	private void OnTriggerExit2D( Collider2D other ) {
		if ( FindObject(other) ) {
			mark.SetActive(false);
		}
	}

	private bool FindObject( Collider2D other ) {
		if ( other.CompareTag("OBJECT") )
			return true;

		return false;
	}
}
