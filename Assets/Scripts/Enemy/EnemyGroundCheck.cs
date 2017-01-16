using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour {


    private Animator animator;

    // Use this for initialization
    void Start() {
        this.animator = this.gameObject.transform.parent.gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Ground>() != null) {
            animator.SetBool("isGrounded", true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        animator.SetBool("isGrounded", false);
    }
}