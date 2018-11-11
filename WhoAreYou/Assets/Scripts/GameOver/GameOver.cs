using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour {

    public static bool game_over;

    // Use this for initialization
    private void Awake()
    {
        game_over = false;
    }

    // Update is called once per frame
    void Update () {
		if(game_over && Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("Source");
        }
	}
}
