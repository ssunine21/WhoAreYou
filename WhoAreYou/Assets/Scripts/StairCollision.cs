using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StairCollision : MonoBehaviour
{
	private BoxCollider2D col2D;

	//player가 계단+value 보다 위에 있을 때 충돌을 ture값으로
	readonly float value = 0.5f;

    void Start()
    {
		col2D = GetComponent<BoxCollider2D>();
    }
	//other.gameObject == player.gameObject && 
	private void OnTriggerEnter2D( Collider2D other ) {
		if ( other.transform.position.y > transform.position.y + value )
			col2D.isTrigger = false;
	}


	private void OnCollisionStay2D( Collision2D other ) {
		if ( other.transform.position.y < transform.position.y + value && other.rigidbody.velocity.y == 0 )
			col2D.isTrigger = true;
	}

	private void OnCollisionExit2D( Collision2D other ) {
		col2D.isTrigger = true;
	}
}
