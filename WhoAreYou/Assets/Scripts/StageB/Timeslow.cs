using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeslow : MonoBehaviour {

    public GameObject topstone0;
    public GameObject topstone1;
    public GameObject topstone2;
    public GameObject topstone3;
    public GameObject topstone4;

    bool timeslow = true;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayerBody")
        {
            if (timeslow)
            {
                Time.timeScale = 0.2f;
                topstone0.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                topstone1.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                topstone2.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                topstone3.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                topstone4.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

                monsterMove.player_run = true;

                timeslow = false;
            }

        }
    }
}
