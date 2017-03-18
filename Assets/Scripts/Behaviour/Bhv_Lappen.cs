using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bhv_Lappen : MonoBehaviour {

    private Rigidbody2D me;

	// Use this for initialization
	void Start () {
        me = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GetComponent<Animator>().GetBool("isGrounded"))
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Destroy(this); // destroy bhv_Lappen
        }
        else
        {
            me.velocity = Vector2.up * me.velocity.y;
        }
	}
}
