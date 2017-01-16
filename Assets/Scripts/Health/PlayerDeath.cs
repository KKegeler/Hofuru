using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : Death {

    Animator animator;
    PlayerMovement pm;

	// Use this for initialization
	void Start () {
        this.animator = this.gameObject.GetComponent<Animator>();
        this.pm = this.gameObject.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1) && this.gameObject.GetComponent<PlayerMovement>()) {
            this.gameObject.GetComponent<Health>().ReduceHealth(14);
        }
	}

    public override void HandleDeath() {
        this.pm.enabled = false;
        this.animator.SetBool("isDead", true);
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
