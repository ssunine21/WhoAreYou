using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterA : MonoBehaviour {

    public GameObject Vpad;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            Vpad.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
