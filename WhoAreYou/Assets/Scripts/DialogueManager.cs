using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public GameObject dialogue_touchpad;
    public Text dText;

    public bool dialogActive;
    public int currentLine;

    public string[] dialogLines;
    
    zFoxVirtualPad vpad;
    zFOXVPAD_BUTTON vpad_dialogue;

    // Use this for initialization

    void Start ()
    {
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
        dialogue_touchpad.SetActive(false);

        currentLine = 0;
        dialogActive = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        vpad_dialogue = zFOXVPAD_BUTTON.NON;

        if (vpad != null)
            vpad_dialogue = vpad.dialogue;
        

        if (dialogActive && vpad_dialogue == zFOXVPAD_BUTTON.DOWN)
        {
            //dBox.SetActive(false);
            //dialogActive = false;

            currentLine++;
        }

        if (currentLine >= dialogLines.Length)
        {
            dialogue_touchpad.SetActive(false);
            dBox.SetActive(false);
            dialogActive = false;

            currentLine = 0;
        }
        else
            dText.text = dialogLines[currentLine];
	}

    public void ShowBox(string dialogue)
    {
        Debug.Log("show dialogue");
        dialogue_touchpad.SetActive(true);
        dialogActive = true;
        dBox.SetActive(true);
        dialogLines[currentLine] = dialogue;
    }

    public void ShowDialogue()
    {
        Debug.Log("show dialogue");

        dialogue_touchpad.SetActive(true);
        dialogActive = true;
        dBox.SetActive(true);
    }
}
