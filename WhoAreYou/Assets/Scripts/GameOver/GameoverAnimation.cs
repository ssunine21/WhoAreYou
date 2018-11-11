using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverAnimation : MonoBehaviour {

    Animator animator;

    // Use this for initialization
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
		if(GameOver.game_over)
        {
            animator.SetBool("isGameOver", true);
        }
	}
}
