using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour {


    private Animator animator;
    public bool grounded = false;

    // Use this for initialization
    void Start() {
        this.animator = this.gameObject.transform.parent.gameObject.GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Ground>() != null) {
            animator.SetBool("isGrounded", true);
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        animator.SetBool("isGrounded", false);
        grounded = false;
    }
}