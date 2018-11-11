using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LinkStageA : MonoBehaviour {
    
    public string levelToLoad;
    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_btnB;

    // Use this for initialization
    void Start ()
    {
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
    }

    // Update is called once per frame

    private void Update()
    {
        vpad_btnB = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
        {
            vpad_btnB = vpad.buttonB;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("왜안되지");
            if (Input.GetKeyDown(KeyCode.X) || vpad_btnB == zFOXVPAD_BUTTON.DOWN)
            {
                Debug.Log("왜안되지");
                SceneManager.LoadScene(levelToLoad);
            }
        }
    }
}
