using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerDeath>())
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerDeath>())
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
