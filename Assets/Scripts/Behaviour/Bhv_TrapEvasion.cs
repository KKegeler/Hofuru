using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bhv_TrapEvasion : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.tag.Equals("trap"))
            Debug.Log("TRAP DETECTED!");
    }

}
