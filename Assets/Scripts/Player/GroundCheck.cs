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

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.GetComponent<Ground>() != null) {
            print("Grounded");
            pm.SetisGrounded(true);
        } else {
            pm.SetisGrounded(false);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        pm.SetisGrounded(false);

        print("air");
    }
}
