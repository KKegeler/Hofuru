using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {

    //private Collider2D trigger;
    private PlayerMovement pm;

    // Use this for initialization
    void Start() {
        //trigger = GetComponent<Collider2D>();
        pm = GetComponentInParent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        Ground ground = other.gameObject.GetComponent<Ground>();
        
        if (ground)
                pm.SetisGrounded(true);
    }

    void OnCollisionExit2D(Collision2D other) {
        pm.SetisGrounded(false);
    }
}
